using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CollectableHealth : MonoBehaviour
{
    public event Action Collected;

    public void Awake()
    {
        Root.CollectablesManager.Register(this);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<PlayerCharacter>(out _))
        {
            Collected?.Invoke();
            Root.CollectablesManager.Unregister(this);
            Destroy(gameObject);
        }
    }
}
