using Assets.Scripts;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class BulletsTrack : MonoBehaviour, IApplyButton
{
    [SerializeField] private CollectableBullet _bulletPrefab;
    [SerializeField] private ushort _bullets;
    [SerializeField] private float _distance;


    public void Apply()
    {
#if UNITY_EDITOR
        CollectableBullet[] bullets = GetComponentsInChildren<CollectableBullet>();
        var bulletsStack = new Stack<CollectableBullet>(bullets);
        if (bulletsStack.Count > _bullets)
        {
            while (bulletsStack.Count > _bullets)
                DestroyImmediate(bulletsStack.Pop().gameObject);
        }
        else if (bulletsStack.Count < _bullets)
        {
            while (bulletsStack.Count < _bullets)
            {
                Vector3 newPosition = bulletsStack.Count == 0 ? transform.position : bulletsStack.Peek().transform.position + new Vector3(0, 0, _distance);
                CollectableBullet bullet = (CollectableBullet)PrefabUtility.InstantiatePrefab(_bulletPrefab);
                bullet.transform.position = newPosition;
                bullet.transform.SetParent(transform);
                bulletsStack.Push(bullet);
            }
        }
#endif

    }
}
