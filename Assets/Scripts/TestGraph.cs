using UnityEngine;

public class TestGraph : MonoBehaviour
{
    [SerializeField] private AnimationCurve curve;

    void FixedUpdate()
    {
        curve.AddKey(Time.time, transform.position.y);
    }
}
