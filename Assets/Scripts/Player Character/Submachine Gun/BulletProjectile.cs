using Assets.Scripts;
using UnityEngine;

public class BulletProjectile : Projectile
{
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;
    [SerializeField] private ParticleSystem _sparks;

    public override void Init(Vector3 initialPosition, Vector3 target, float targetForwardSpeed)
    {
        base.Init(initialPosition, target, targetForwardSpeed);
        var direction = _start.position - _end.position;
        transform.position = direction + initialPosition;
    }

    protected override void Destroy()
    {
        PoolService.ProjectilePool.Return(this);
    }

    private void OnEnable()
    {
        _time = 0;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Damagable damagable))
        {
            damagable.ApplyDamage(DamageOrigin);
            Destroy();
            Instantiate(_sparks, transform.position, Quaternion.identity);
        }
    }
}

