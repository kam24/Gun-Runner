using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

namespace Assets.Scripts
{
    public class PoolService: MonoBehaviour
    {
        [SerializeField] private GameObjectPool _bulletPool;
        [SerializeField] private ProjectilePool _projectilePool;

        public static GameObjectPool BulletPool { get; private set; }
        public static ProjectilePool ProjectilePool { get; private set; }

        public void Init()
        {
            BulletPool = _bulletPool;
            ProjectilePool = _projectilePool;

            BulletPool.Init();
            ProjectilePool.Init();
        }
    }
}
