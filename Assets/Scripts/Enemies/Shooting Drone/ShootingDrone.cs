using Assets.Scripts;
using DroneStateMachine;
using DroneStateMachine.States;
using System;
using System.Collections;
using UnityEngine;

public class ShootingDrone : Damagable
{
    [SerializeField] private DroneHealthView _healthView;
    [SerializeField][Range(0, 45)] private float _maxRotationDownX;
    [SerializeField] private DroneProjectile _projectile;
    [SerializeField] private Transform _shootingPoint;
    [SerializeField][Min(0.01f)] private float _appearanceSpeed;
    [SerializeField] private uint _maxProjectiles;
    [SerializeField][Min(0.01f)] private float _reloadingTime;
    [SerializeField] private float _targetDistance;
    [SerializeField] private float _heightAboveTarget;
    [SerializeField] private float _appereanceHeight;
    [SerializeField] private float _targetPointY;
    [SerializeField][Min(0)] private float _dyingRotationSpeed;
    [SerializeField][Min(0)] private float _fallingDownSpeed;
    [SerializeField][Min(0)] private float _delayBeforeFallingDown;
    [SerializeField][Range(0, 1)] private float _startBurningHealthRatio;
    [SerializeField] private ParticleSystem _smoke;
    [SerializeField] private ParticleSystem _burning;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private LayerMask _layersToExplode;
    [SerializeField] private AudioSource _shootSound;

    public event Action Shot;

    public float AppearanceSpeed => _appearanceSpeed;
    public float HeightAboveTarget => _heightAboveTarget;
    public uint MaxProjectiles => _maxProjectiles;
    public float ReloadingTime => _reloadingTime;
    public float DelayBeforeFallingDown => _delayBeforeFallingDown;

    public DroneStateMachine.DroneSM StateMachine { get; private set; }

    private PlayerCharacter _target;
    private Vector3 TargetPosition => _target.transform.position;
    private float lastTargetPositionZ;

    public void Init(PlayerCharacter target)
    {
        _target = target;
        _target.Killed += OnTargetKilled;

        base.Init(new Health(90));
        Health.Changed += OnHealthChanged;
        Health.Dying += OnDying;

        var x = 0;
        var y = _heightAboveTarget + _appereanceHeight + TargetPosition.y;
        var z = _targetDistance + TargetPosition.z;
        transform.position = new Vector3(x, y, z);
        lastTargetPositionZ = TargetPosition.z;

        StateMachine = new DroneSM(this);
        StateMachine.Start<AppearanceState>();
    }

    private void OnTargetKilled()
    {
        StateMachine.Finish();
    }

    private void OnDisable()
    {
        _target.Killed -= OnTargetKilled;
    }

    private void OnDying()
    {
        Instantiate(_burning, transform);
        Health.Dying -= OnDying;
    }

    private void FixedUpdate()
    {
        StateMachine.FixedUpdate(Time.fixedDeltaTime);
    }

    public void TurnTowardsTarget()
    {
        var direction = TargetPosition - transform.position;
        var angleY = Mathf.Atan2(direction.x, direction.z) * Mathf.Rad2Deg;
        var angleX = Mathf.Atan2(direction.y, direction.z) * Mathf.Rad2Deg + 180;
        angleX = Mathf.Clamp(angleX, 0, _maxRotationDownX);
        transform.rotation = Quaternion.Euler(angleX, angleY, 0);
    }

    public void MoveForward()
    {
        float offsetZ = lastTargetPositionZ - TargetPosition.z;
        transform.Translate(0, 0, -offsetZ, Space.World);
        lastTargetPositionZ = TargetPosition.z;
    }

    public void RotateAroundItself()
    {
        transform.rotation *= Quaternion.AngleAxis(_dyingRotationSpeed, Vector3.up);
    }

    public void FallDownOnTargetLine()
    {
        int k = 0;
        var delta = transform.position.x - TargetPosition.x;
        if (delta > 0)
            k = -1;
        else if (delta < 0)
            k = 1;
        var x = k * _fallingDownSpeed * Time.deltaTime;
        var y = -_fallingDownSpeed * Time.deltaTime;
        transform.Translate(x, y, 0, Space.World);
    }

    public void DisableHealthBar()
    {
        _healthView.gameObject.SetActive(false);
    }

    public IEnumerator Shoot()
    {
        while (true)
        {
            var projectile = Instantiate<DroneProjectile>(_projectile);
            projectile.Init(_shootingPoint.position, TargetPosition + _targetPointY * Vector3.up, _target.ForwardSpeed);
            _shootSound.Play();
            Shot?.Invoke();
            yield return new WaitForSeconds(2f);
        }
    }

    public IEnumerator StartTimer(float time, Action callBack)
    {
        yield return new WaitForSeconds(time);
        callBack?.Invoke();
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (UtilityHelper.IsEqualLayers(_layersToExplode, collision.gameObject.layer))
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
            Destroy(gameObject);
        }
    }

    private void OnHealthChanged()
    {
        if (Health.Value / Health.MaxValue < _startBurningHealthRatio)
        {
            Instantiate(_smoke, transform);
            Health.Changed -= OnHealthChanged;
        }
    }
}