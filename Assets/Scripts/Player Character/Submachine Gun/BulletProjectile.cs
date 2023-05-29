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

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Damagable damagable))
        {
            damagable.ApplyDamage(DamageOrigin);
            Destroy(gameObject);
            Instantiate(_sparks, transform.position, Quaternion.identity);
        }
    }
}

