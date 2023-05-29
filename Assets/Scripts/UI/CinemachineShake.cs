using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

[RequireComponent(typeof(CinemachineVirtualCamera))]
public class CinemachineShake : MonoBehaviour
{
    [SerializeField][Min(0)] private float _hitShakeIntensity;
    [SerializeField][Min(0)] private float _hitShakeTime;
    [SerializeField][Min(0)] private float _maxDistanceToExplosion;
    [SerializeField][Min(0)] private float _explosionNearbyIntensity;
    [SerializeField][Min(0)] private float _explosionNearbyTime;
    [SerializeField][Min(0)] private float _bigExplosionCoefficient;

    private CinemachineBasicMultiChannelPerlin _channelsPerlin;
    private PlayerCharacter _character;
    private Coroutine _reducer;

    public void Init(PlayerCharacter playerCharacter)
    {
        _channelsPerlin = GetComponent<CinemachineVirtualCamera>().GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
        _character = playerCharacter;
        _character.OnProjectileHit += OnHitShake;
        enabled = true;
    }

    public void OnExplosionNearby(float distance, DamageOrigin damageOrigin)
    {
        if (_reducer != null)
            StopCoroutine(_reducer);
        var intensity = _explosionNearbyIntensity * (1 - Mathf.Clamp(distance, 0, _maxDistanceToExplosion) / _maxDistanceToExplosion);
        var time = _explosionNearbyTime;
        if (damageOrigin == DamageOrigin.DroneExplosion)
        {
            intensity *= _bigExplosionCoefficient;
            time = _hitShakeTime;
        }
        ShakeCamera(intensity, time);
    }

    private void OnDisable()
    {
        _character.OnProjectileHit -= OnHitShake;
    }

    private void OnHitShake()
    {
        ShakeCamera(_hitShakeIntensity, _hitShakeTime);
    }

    private void ShakeCamera(float intensity, float time)
    {
        _channelsPerlin.m_AmplitudeGain = intensity;
        _reducer = StartCoroutine(ReduceShakeWithTime(time));
    }

    public IEnumerator ReduceShakeWithTime(float duration)
    {
        var startValue = _channelsPerlin.m_AmplitudeGain;
        var time = 0f;
        while (time <= duration)
        {
            _channelsPerlin.m_AmplitudeGain = Mathf.Lerp(startValue, 0, time / duration);
            time += Time.deltaTime;
            yield return null;
        }
        _channelsPerlin.m_AmplitudeGain = 0;
        _reducer = null;
    }
}
