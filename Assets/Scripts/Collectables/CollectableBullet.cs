using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CollectableBullet : PoolObject
    {
        [SerializeField] private ushort _count;

        public event Action<ushort> Collected;        

        private void OnEnable()
        {
            Root.CollectablesManager.Register(this);
        }

        private void OnDisable()
        {
            Root.CollectablesManager.Unregister(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<PlayerCharacter>(out _))
            {
                Collected?.Invoke(_count);
                ReturnToPool();
            }
        }
    }
}
