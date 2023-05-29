using System;
using UnityEngine;

namespace Assets.Scripts
{
    public class CollectableBullet : MonoBehaviour
    {
        [SerializeField] private ushort _count;

        public event Action<ushort> Collected;

        public void Awake()
        {
            Root.CollectablesManager.Register(this);
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.gameObject.TryGetComponent<PlayerCharacter>(out _))
            {
                Collected?.Invoke(_count);
                Root.CollectablesManager.Unregister(this);
                Destroy(gameObject);
            }
        }
    }
}
