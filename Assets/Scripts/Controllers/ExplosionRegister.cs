using UnityEngine;

public class ExplosionRegister
{
    private PlayerCharacter _character;
    private CinemachineShake _cinemachineShake;

    public ExplosionRegister(PlayerCharacter character, CinemachineShake cinemachineShake)
    {
        _character = character;
        _cinemachineShake = cinemachineShake;
    }

    public void Register(Vector3 position, DamageOrigin damageOrigin)
    {
        float distance = (position - _character.transform.position).magnitude;
        _cinemachineShake.OnExplosionNearby(distance, damageOrigin);
    }
}