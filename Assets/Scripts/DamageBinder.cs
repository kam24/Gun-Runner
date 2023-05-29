using System;
using System.Collections.Generic;

public static class DamageBinder
{
    private static List<Damage> _damageList;

    static DamageBinder()
    {
        _damageList = new List<Damage>()
        {
            new Damage(DamageOrigin.DroneProjectile, DamageReceiver.Player, 1),
            new Damage(DamageOrigin.ProjectileExplosion, DamageReceiver.Player, 1),
            new Damage(DamageOrigin.DroneExplosion, DamageReceiver.Player, 3),
            new Damage(DamageOrigin.BulletProjectile, DamageReceiver.ShootingDrone, 1)
        };
    }

    public static ushort ApplyDamage(DamageOrigin origin, DamageReceiver receiver)
    {
        foreach (var damage in _damageList)
            if (damage.Origin == origin && damage.Receiver == receiver)
                return damage.Value;

        throw new ArgumentException();
    }
}

public class Damage
{
    public DamageOrigin Origin { get; private set; }
    public DamageReceiver Receiver { get; private set; }
    public ushort Value { get; private set; }

    public Damage(DamageOrigin origin, DamageReceiver receiver, ushort value)
    {
        Origin = origin;
        Receiver = receiver;
        Value = value;
    }
}

public enum DamageOrigin
{
    DroneProjectile,
    BulletProjectile,
    ProjectileExplosion,
    DroneExplosion
}

public enum DamageReceiver
{
    Player,
    ShootingDrone
}

