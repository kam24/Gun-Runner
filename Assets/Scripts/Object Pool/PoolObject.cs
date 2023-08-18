using System;
using UnityEngine;

public abstract class PoolObject : MonoBehaviour
{
    private GameObjectPool _pool;

    public void SetPool(GameObjectPool pool)
    {
        if (_pool == null)
            _pool = pool;
        else
            throw new InvalidOperationException();
    }

    public void ReturnToPool()
    {
        _pool.Return(this);
    }
}

