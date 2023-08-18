using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UIElements;

public class ProjectilePool : GameObjectPool
{
    public override void Init()
    {
        if (_prefab.TryGetComponent(out Projectile _) == false)
            throw new InvalidCastException(nameof(_prefab));

        base.Init();
    }

    public Projectile Get(Vector3 position, Vector3 target, float targetForwardSpeed)
    {
        var projectile = Get(position) as Projectile;
        projectile.Init(position, target, targetForwardSpeed);
        return projectile;
    }
}