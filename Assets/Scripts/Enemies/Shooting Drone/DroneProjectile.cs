using System;
using UnityEngine;

[RequireComponent(typeof(SphereCollider))]
public class DroneProjectile : Projectile
{
    [SerializeField] private Explosion _explosion;
    [SerializeField] private ParticleSystem _effect;
    [SerializeField] private ParticleSystem _trail;
    [SerializeField] private AudioSource _explosionSound;

    public override void Init(Vector3 initialPosition, Vector3 target, float targetForwardSpeed)
    {
        base.Init(initialPosition, target, targetForwardSpeed);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent(out Damagable damagable))
        {
            damagable.ApplyDamage(DamageOrigin);
            var effect = Instantiate(_effect, transform.position, Quaternion.identity);
            var sound = Instantiate(_explosionSound, transform.position, Quaternion.identity);
            sound.transform.SetParent(effect.transform, false);
            sound.Play();
        }
        else
        {
            Instantiate(_explosion, transform.position, Quaternion.identity);
        }
        _trail.transform.SetParent(null, false);
        _trail.transform.position = gameObject.transform.position;
        _trail.maxParticles = 0;
        var velocity = _trail.velocityOverLifetime;
        velocity.enabled = false;
        Destroy(gameObject);
    }
}
