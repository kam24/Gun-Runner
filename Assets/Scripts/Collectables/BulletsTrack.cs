using Assets.Scripts;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class BulletsTrack : MonoBehaviour, IApplyButton
{
    [SerializeField] private GameObject _bulletPoint;
    [SerializeField] private ushort _bullets;
    [SerializeField] private float _distance;

    public void Apply()
    {
        List<Transform> childs = GetChilds();
        childs?.ForEach(child => DestroyImmediate(child.gameObject));

        Vector3 bulletPosition = transform.position;
        for (int i = 0; i < _bullets; i++)
        {
            GameObject bulletPoint = Instantiate(_bulletPoint);
            bulletPoint.transform.position = bulletPosition;
            bulletPoint.transform.SetParent(transform);

            bulletPosition += new Vector3(0, 0, _distance);
        }
    }

    public void Init()
    {
        List<Transform> childs = GetChilds();
        childs.ForEach(child =>
        {
            PoolService.BulletPool.Get(child.position);
        });
    }

    private void OnDisable()
    {
        var childs = GetComponentsInChildren<CollectableBullet>().ToList();
        childs.ForEach(child =>
        {
            if (child.gameObject.activeInHierarchy)
                child.ReturnToPool();
        });
    }

    private List<Transform> GetChilds()
    {
        return transform.Cast<Transform>().ToList();
    }
}
