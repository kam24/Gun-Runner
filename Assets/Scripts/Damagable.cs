using Assets.Scripts;
using UnityEngine;

public abstract class Damagable: MonoBehaviour
{
    [Header("Damage")]
    [SerializeField] private DamageReceiver _damageReceiver;

    public Health Health { get; private set; }
    
    public void Init(Health health)
    {
        Health = health;
    }

    public virtual void ApplyDamage(DamageOrigin origin)
    {
        var damage = DamageBinder.ApplyDamage(origin, _damageReceiver);
        Health.Decrease(damage);
    }

    protected void ApplyDamage(ushort value)
    {
        Health.Decrease(value);
    }
}

