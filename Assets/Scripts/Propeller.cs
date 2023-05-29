using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Propeller : MonoBehaviour
{
    [SerializeField] private float _rotationSpeed;

    private void FixedUpdate()
    {
        transform.rotation *= Quaternion.Euler(_rotationSpeed * Vector3.right);
    }
}
