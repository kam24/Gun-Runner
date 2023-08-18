using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class GameObjectPool: MonoBehaviour
{
    [SerializeField] protected PoolObject _prefab;
    [SerializeField] private int _capacity;
    [SerializeField] private int _maxCapacity;

    protected List<PoolObject> _objects;

    public virtual void Init()
    {
        _objects = new List<PoolObject>(_maxCapacity);
        for (int i = 0; i < _capacity; i++)
            AddObject();
    }

    public PoolObject Get(Vector3 position)
    {
        PoolObject go = _objects.FirstOrDefault(go => go.gameObject.activeInHierarchy == false);

        if (go == null)
            go = AddObject();

        go.transform.position = position;
        go.gameObject.SetActive(true);
        return go;
    }

    public void Return(PoolObject gameObject)
    {
        PoolObject returningObject = _objects.FirstOrDefault(go => go == gameObject);

        if (returningObject != null)
            returningObject.gameObject.SetActive(false);
        else
            throw new InvalidOperationException();
    }

    protected PoolObject AddObject()
    {
        PoolObject prefab = Instantiate(_prefab, transform);
        prefab.gameObject.SetActive(false);
        _objects.Add(prefab);
        prefab.SetPool(this);

        return prefab;
    }
}

