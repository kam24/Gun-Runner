using Assets.Scripts;
using PlayerMovement;
using PlayerMovement.States;
using System;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(Animator), typeof(CharacterController))]
public class PlayerCharacter : Damagable
{
    [Header("Speed")]
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _horizontalSpeed;
    [SerializeField] private float _fallDownSpeed;
    [SerializeField] private float _wallRunFallDownSpeed;
    [Header("Jump")]
    [SerializeField] private float _jumpHeight = 2;
    [SerializeField] private float _gravity = -9.81f;
    [Header("Wall Run")]
    [SerializeField] private float _checkWallDistance;
    [SerializeField] private float _wallRunningGap;
    [SerializeField] private float _wallRunHeight = 1f;
    [SerializeField] private LayerMask _wallMask;
    [Header("Ground")]
    [SerializeField] private float _groundedOffset = -0.14f;
    [SerializeField] private float _groundedRadius = 0.28f;
    [SerializeField] private LayerMask _groundLayers;
    [Header("")]
    [SerializeField] private LayerMask _staticEnvironmentMask;
    [SerializeField] private float _timeToRegeneration;
    [SerializeField] private float _readyToShotTime;
    [Header("Gun")]
    [SerializeField] private ParticleSystem _gunFlash;
    [SerializeField][Min(0.01f)] private float rateOfFirePerSecond;
    [SerializeField] private float _readyToAimTime;
    [SerializeField] private BulletProjectile _bulletProjectile;
    [SerializeField] private Transform _shootingPoint;
    [Header("VFX")]
    [SerializeField] private GameObject _bulletCollectedVFX;
    [SerializeField] private float _VFXDuration;
    [Header("SFX")]
    [SerializeField] private AudioSource _bulletCollectedSound;
    [SerializeField] private AudioSource _shootSound;
    [SerializeField] private AudioSource _wallHitSound;

    public enum Strafe
    {
        Left = -1,
        None = 0,
        Right = 1
    }

    public enum WallRun
    {
        Left = Strafe.Left,
        None = Strafe.None,
        Right = Strafe.Right
    }

    public enum InjuryReason
    {
        None,
        ForwardObstacle,
        RightObstacle,
        LeftObstacle,
        BigExplosion,
        Other
    }

    public event Action OnProjectileHit;
    public event Action Killed;

    public int AnimIDGrounded { get; private set; }
    public int AnimIDJump { get; private set; }
    public int AnimIDRoll { get; private set; }
    public int AnimIDWallRun { get; private set; }
    public int AnimIDStrafeDirection { get; private set; }
    public int AnimIDShooting { get; private set; }
    public int AnimIDSideDeath { get; private set; }
    public int AnimIDObstacleDeath { get; private set; }
    public int AnimIDCommonDeath { get; private set; }
    public int AnimIDSideObstacle { get; private set; }
    public int AnimIDIsActive { get; private set; }

    public RunLine RunLine { get; private set; }
    public Strafe LastStrafe { get; set; }
    public WallRun WallRunning { get; set; }

    public float HorizontalSpeed => _horizontalSpeed;
    public float ForwardSpeed => _forwardSpeed;
    public float WallRunFallDownSpeed => _wallRunFallDownSpeed;
    public float WallRunHeight => _wallRunHeight;

    public event Action TargetAppeared;
    public bool IsTargetAppeared { get; private set; }

    private CharacterController _controller;
    private float _normalHeight;
    private Vector3 _normalCenter;
    private float _currentGravity;
    private float NormalGravity => _gravity;

    private RaycastHit _wallHit;
    private float _verticalVelocity;
    private float _wallCheckRayDistance;

    private Animator _animator;
    private PlayerStateMachine _stateMachine;
    private bool _inputEnabled;

    private Transform _target;

    private Coroutine _regenerationTimer;

    private InjuryReason _lastInjuryReason;

    public void Init(RunLine runLine, Health health)
    {
        RunLine = (RunLine)runLine.Clone();
        base.Init(health);
        _lastInjuryReason = InjuryReason.None;

        _animator = GetComponent<Animator>();
        _controller = GetComponent<CharacterController>();
        _controller.enabled = true;

        _normalHeight = _controller.height;
        _normalCenter = _controller.center;
        _currentGravity = _gravity;
        SetNormalRayDistance();

        InitAnimationIDs();

        _stateMachine = new PlayerStateMachine(this);
        _stateMachine.Start<RunningState>();

        SetAnimationBool(AnimIDIsActive, true);
        _inputEnabled = true;
        enabled = true;
    }

    public override void ApplyDamage(DamageOrigin origin)
    {
        if (origin == DamageOrigin.ProjectileExplosion || origin == DamageOrigin.DroneProjectile)
            OnProjectileHit?.Invoke();

        _lastInjuryReason = origin switch
        {
            DamageOrigin.DroneExplosion => InjuryReason.BigExplosion,
            _ => InjuryReason.Other,
        };
        base.ApplyDamage(origin);
    }

    private void InitAnimationIDs()
    {
        AnimIDGrounded = Animator.StringToHash("Grounded");
        AnimIDJump = Animator.StringToHash("Jump");
        AnimIDRoll = Animator.StringToHash("Roll");
        AnimIDWallRun = Animator.StringToHash("Wall Run");
        AnimIDStrafeDirection = Animator.StringToHash("Strafe Direction");
        AnimIDShooting = Animator.StringToHash("Shooting");
        AnimIDSideDeath = Animator.StringToHash("Side Death");
        AnimIDObstacleDeath = Animator.StringToHash("Obstacle Death");
        AnimIDCommonDeath = Animator.StringToHash("Common Death");
        AnimIDSideObstacle = Animator.StringToHash("Side Obstacle");
        AnimIDIsActive = Animator.StringToHash("Is Active");
    }

    private void FixedUpdate()
    {
        float time = Time.fixedDeltaTime;

        _stateMachine.FixedUpdate(time);
        ApplyGravity(time);
        MoveForward(time);

        Vector3 origin = transform.position + _controller.height / 10 * Vector3.up;
        Debug.DrawRay(origin, _wallCheckRayDistance * -transform.right);
        Debug.DrawRay(origin, _wallCheckRayDistance * transform.right);
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (UtilityHelper.IsEqualLayers(_staticEnvironmentMask.value, hit.gameObject.layer) == false)
            return;

        if (Math.Abs(hit.normal.x) == 1)
        {
            _lastInjuryReason = LastStrafe == Strafe.Right ? InjuryReason.RightObstacle : InjuryReason.LeftObstacle;

            LastStrafe = Strafe.None;
            _stateMachine.PushExtraState<StrafeOnLineState>();

            ApplyDamage(2);

            _wallHitSound.Play();
        }

        if (Math.Abs(hit.normal.z) == 1)
        {
            _lastInjuryReason = InjuryReason.ForwardObstacle;
            ApplyDamage(2);

            _wallHitSound.Play();
        }
    }

    private void OnEnable()
    {
        Health.Changed += OnHealthChanged;
        Health.Dying += Kill;
    }

    private void OnDisable()
    {
        Health.Changed -= OnHealthChanged;
        Health.Dying -= Kill;
    }

    private void MoveForward(float time)
    {
        float z = _forwardSpeed * time;
        Vector3 offset = new(0, 0, z);
        _controller.Move(offset);
    }

    private void ApplyGravity(float time)
    {
        _verticalVelocity += _currentGravity * time;
        _controller.Move(_verticalVelocity * time * Vector3.up);
    }

    public void SetJumpVelocity(float height)
    {
        _verticalVelocity = (float)Math.Sqrt(-2 * height * _gravity);
    }

    public void ApplyMaxDamage()
    {
        ApplyDamage((ushort)Health.MaxValue);
    }

    private void Kill()
    {
        Health.Changed -= OnHealthChanged;
        Health.Dying -= Kill;
        _inputEnabled = false;
        _forwardSpeed = 0;
        _stateMachine.Finish();
        switch (_lastInjuryReason)
        {
            case InjuryReason.ForwardObstacle or InjuryReason.BigExplosion:
                SetAnimationTrigger(AnimIDObstacleDeath);
                break;
            case InjuryReason.LeftObstacle:
                SetAnimationTrigger(AnimIDSideDeath);
                SetAnimationBool(AnimIDSideObstacle, true);
                break;
            case InjuryReason.RightObstacle:
                SetAnimationTrigger(AnimIDSideDeath);
                SetAnimationBool(AnimIDSideObstacle, false);
                break;
            case InjuryReason.Other:
                SetAnimationTrigger(AnimIDCommonDeath);
                break;
        }
        Killed?.Invoke();
    }

    public void Move(float x, float y = 0)
    {
        var offset = new Vector3(x, y, 0);
        _controller.Move(offset);
    }

    public void SetZeroGravity()
    {
        _currentGravity = 0f;
        _verticalVelocity = 0f;
    }

    public void SetNormalGravity()
    {
        _currentGravity = NormalGravity;
    }

    public void SetGroundedVelocity()
    {
        _verticalVelocity = -2f;
    }

    public void SetFastFallDownVelocity()
    {
        _verticalVelocity = -_fallDownSpeed;
    }

    public void SetNormalRayDistance()
    {
        _wallCheckRayDistance = RunLine.Offset * 1.5f;
    }

    public void SetLongRayDistance()
    {
        _wallCheckRayDistance = RunLine.Offset * 2f;
    }

    public void SetAnimationTrigger(int id)
    {
        _animator.SetTrigger(id);
    }

    public void SetAnimationBool(int id, bool value)
    {
        _animator.SetBool(id, value);
    }

    public void OnUpKey()
    {
        if (_inputEnabled)
            _stateMachine.HandleUpKey();
    }

    public void OnDownKey()
    {
        if (_inputEnabled)
            _stateMachine.HandleDownKey();
    }

    public void OnLeftRightKey(Strafe strafe)
    {
        if (_inputEnabled)
            _stateMachine.HandleLeftRightKey(strafe);
    }

    public void Jump()
    {
        SetAnimationTrigger(AnimIDJump);
        _verticalVelocity = (float)Math.Sqrt(-2 * _jumpHeight * _gravity);
    }

    public void JumpForStrafe(Strafe strafe)
    {
        SetAnimationTrigger(AnimIDJump);
        float? endPosition;
        if (_wallHit.collider != null)
            endPosition = GetWallRunPositionX(strafe);
        else if (strafe == Strafe.Left)
            endPosition = RunLine.GetLeft();
        else if (strafe == Strafe.Right)
            endPosition = RunLine.GetRight();
        else
            throw new InvalidOperationException();

        float distance = (float)Math.Abs((double)(transform.position.x - endPosition));
        float time = distance / HorizontalSpeed;
        float height = transform.position.y < _wallRunHeight ? _wallRunHeight - transform.position.y : 0;
        _verticalVelocity = Math.Abs(_currentGravity) * time / 2 + height / time;
    }

    public bool CheckWallAhead(Strafe strafe)
    {
        bool isWallHitByOffset = false;
        int direction = (int)strafe;

        _wallHit = new RaycastHit();
        Vector3 origin = transform.position + _controller.height / 10 * Vector3.up;
        Vector3 offsetOrigin = origin + RunLine.Offset * Vector3.forward;
        bool isWallHitByRunLineOffset = Physics.Raycast(offsetOrigin, direction * transform.right, out _wallHit, _wallCheckRayDistance, _wallMask);

        if (isWallHitByRunLineOffset)
        {
            float offsetTime = Math.Abs(GetWallRunPositionX(strafe) - transform.position.x) / _horizontalSpeed;
            Vector3 offset = (_forwardSpeed * offsetTime) * Vector3.forward;
            offsetOrigin = origin + offset;
            isWallHitByOffset = Physics.Raycast(offsetOrigin, direction * transform.right, out _wallHit, _wallCheckRayDistance, _wallMask);
        }

        return isWallHitByOffset;
    }

    public bool CheckRunningWall()
    {
        if (_wallHit.collider == null || WallRunning == WallRun.None || LastStrafe == Strafe.None)
            throw new InvalidOperationException();

        int direction = (int)LastStrafe;
        float distance = RunLine.Offset;

        Vector3 origin = transform.position + _controller.height / 10 * Vector3.up;
        bool isWallHit = Physics.Raycast(origin, direction * transform.right, out _wallHit, distance, _wallMask);
        return isWallHit;
    }

    public float GetWallRunPositionX(Strafe strafe)
    {
        int direction = (int)strafe;
        Transform wall;

        if (_wallHit.collider != null)
            wall = _wallHit.transform;
        else
            throw new InvalidOperationException();

        return wall.position.x + -direction * (wall.localScale.x + _wallRunningGap);
    }

    public bool IsGrounded()
    {
        Vector3 spherePosition = new(transform.position.x, transform.position.y - _groundedOffset, transform.position.z);
        bool grounded = Physics.CheckSphere(spherePosition, _groundedRadius, _groundLayers, QueryTriggerInteraction.Ignore);
        SetAnimationBool(AnimIDGrounded, grounded);
        return grounded;
    }

    public bool IsGround => _controller.isGrounded;

    public void SetShootingTarget(Transform target)
    {
        _target = target;
        IsTargetAppeared = true;
        TargetAppeared?.Invoke();
    }

    public void ResetShootingTarget()
    {
        _target = null;
        IsTargetAppeared = false;
    }

    public Coroutine StartReadyToAimTimer(Action callBack)
    {
        return StartCoroutine(StartTimer(_readyToAimTime, callBack));
    }

    public Coroutine StartReadyToShotTimer(Action callBack)
    {
        return StartCoroutine(StartTimer(_readyToShotTime, callBack));
    }

    public IEnumerator StartTimer(float time, Action callBack)
    {
        yield return new WaitForSeconds(time);
        callBack?.Invoke();
    }

    int x = 0;
    public IEnumerator StartShooting()
    {
        x++;
        //if (x > 1)
        //    throw new InvalidOperationException();
        while (Root.SubmachineGun.HasBullets && IsTargetAppeared)
        {
            var projectile = Instantiate<BulletProjectile>(_bulletProjectile);
            projectile.Init(_shootingPoint.position, _target.position, _forwardSpeed);
            Root.SubmachineGun.Shoot();

            var flash = Instantiate(_gunFlash, _shootingPoint.position, Quaternion.identity);
            flash.transform.SetParent(_shootingPoint);

            _shootSound.Play();

            yield return new WaitForSeconds(1 / rateOfFirePerSecond);
        }
        x--;
    }

    private void OnHealthChanged()
    {
        if (Health.Value == 0 && _regenerationTimer != null)
        {
            StopCoroutine(_regenerationTimer);
            return;
        }

        if (Health.Value < Health.MaxValue)
        {
            if (_regenerationTimer != null)
                StopCoroutine(_regenerationTimer);
            _regenerationTimer = StartCoroutine(StartTimer(_timeToRegeneration, () => Health.RestoreToMax()));
        }
    }

    public void OnBulletCollected()
    {
        _bulletCollectedSound.Play();
        _bulletCollectedVFX.SetActive(true);
        StartCoroutine(StartTimer(_VFXDuration,
            () => _bulletCollectedVFX.SetActive(false)));
    }

    private void SetLowHeight()
    {
        _controller.height /= 2.5f;
        _controller.center /= 2.5f;
    }

    public void SetNormalHeight()
    {
        _controller.height = _normalHeight;
        _controller.center = _normalCenter;
    }

    #region Animation event handlers
    private void OnRollingBegin()
    {
        SetLowHeight();
    }

    private void OnRollingEnd()
    {
        _stateMachine.OnRollingEnd();
    }

    private void OnStartShooting()
    {
        //StartingShooting?.Invoke();
    }

    #endregion
}
