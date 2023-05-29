using System;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private ShootingDrone _shootingDrone;

    public event Action DroneSpawned;
    public event Action DroneDying;

    private PlayerCharacter _player;
    private ShootingDrone _currentDrone;

    public void Init(PlayerCharacter playerCharacter)
    {
        _player = playerCharacter;
    }

    public ShootingDrone SpawnShootingDrone()
    {
        _currentDrone = Instantiate(_shootingDrone);
        _currentDrone.Init(_player);
        _currentDrone.StateMachine.SwitchingToAttackState += OnAttackingDrone;
        _currentDrone.StateMachine.SwitchingToDyingState += OnDyingDrone;
        DroneSpawned?.Invoke();
        return _currentDrone;
    }

    private void OnDyingDrone()
    {
        _currentDrone.StateMachine.SwitchingToDyingState -= OnDyingDrone;
        _player.ResetShootingTarget();
        DroneDying?.Invoke();
    }

    private void OnAttackingDrone()
    {
        _currentDrone.StateMachine.SwitchingToAttackState -= OnAttackingDrone;
        _player.SetShootingTarget(_currentDrone.transform);
    }
}

