using System;
using UnityEngine;

public class Chunk : MonoBehaviour
{
    [SerializeField] private Transform _start;
    [SerializeField] private Transform _end;

    public Vector3 Start => _start.position;
    public Vector3 End => _end.position;

    private void Awake()
    {
        if (Start.z >= End.z)
            throw new InvalidOperationException(gameObject.name);
    }
}