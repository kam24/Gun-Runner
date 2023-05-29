using System;
using TMPro;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private DamageOrigin _damageOrigin;
    [SerializeField][Min(0)] private float _speed;
    [SerializeField][Min(0.01f)] private float _lifetime;
    [SerializeField] private Vector3 _addRotation;

    protected DamageOrigin DamageOrigin => _damageOrigin;

    private Vector3 _initialPosition;
    private Vector3 _target;
    private Vector3 _direction;

    private float _time = 0;

    public virtual void Init(Vector3 initialPosition, Vector3 target, float targetForwardSpeed)
    {
        transform.position = initialPosition;
        _initialPosition = initialPosition;
        _target = target;
        Vector3 projectileVelocity = (_target - _initialPosition).normalized * _speed;
        Vector3 targetVelocity = targetForwardSpeed * Vector3.forward;
        _direction = (targetVelocity + projectileVelocity);
        Quaternion addRotation = Quaternion.Euler(_addRotation);
        transform.rotation = addRotation * Quaternion.LookRotation(_direction);
    }

    private void FixedUpdate()
    {
        transform.Translate(Time.fixedDeltaTime * _direction, Space.World);
    }

    private void Update()
    {
        if (_time > _lifetime)
            Destroy(gameObject);
        _time += Time.deltaTime;
    }
}

