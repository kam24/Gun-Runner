using Cinemachine;
using UnityEngine;

public class CinemachineCameraController : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera _virtualCamera;

    private PlayerCharacter _playerCharacter;
    private CinemachineFramingTransposer _framingTransposer;
    private float _playerGrondedHeight;

    public void Init(PlayerCharacter player)
    {
        _playerCharacter = player;
        _playerCharacter.Killed += DetachFromPlayer;
        _framingTransposer = _virtualCamera.GetCinemachineComponent<CinemachineFramingTransposer>();
        _framingTransposer.m_MinimumDistance = 9f;
        _framingTransposer.m_MaximumDistance = 11f;

        enabled = true;
    }

    private void FixedUpdate()
    {
        float offsetX = -_playerCharacter.transform.position.x / _playerCharacter.RunLine.Offset;
        ChangeTrackableOffset(offsetX);

        var _playerPositionY = _playerCharacter.transform.position.y;
        UnfollowPlayerPositionY();

        if (_playerPositionY < _playerGrondedHeight)
        {
            _playerGrondedHeight = _playerPositionY;
            FollowPlayerPositionY();
        }

        if (_playerCharacter.IsGrounded() || _playerCharacter.WallRunning != PlayerCharacter.WallRun.None)
        {
            _playerGrondedHeight = _playerPositionY;
            FollowPlayerPositionY();
        }
    }

    private void OnDisable()
    {
        _playerCharacter.Killed -= DetachFromPlayer;
    }

    private void DetachFromPlayer()
    {
        _playerCharacter.Killed -= DetachFromPlayer;
        _virtualCamera.Follow = null;
    }

    private void ChangeTrackableOffset(float offsetX)
    {
        Vector3 currentOffset = _framingTransposer.m_TrackedObjectOffset;
        _framingTransposer.m_TrackedObjectOffset = new Vector3(offsetX, currentOffset.y, currentOffset.z);
    }

    private void FollowPlayerPositionY()
    {
        _framingTransposer.m_YDamping = 0.5f;
    }

    private void UnfollowPlayerPositionY()
    {
        _framingTransposer.m_YDamping = 3f;
    }

}
