using System.Collections;
using UnityEngine;

[RequireComponent(typeof(BoxCollider), typeof(AudioSource))]
public class UAV : MonoBehaviour
{
    [SerializeField] private GameObject _model;
    [SerializeField] private float _forwardSpeed;
    [SerializeField] private float _rotationSpeed;
    [SerializeField] private Vector3 _rotationColliderSize;
    [SerializeField] private Explosion _explosion;
    [SerializeField] private float _lifetime;
    [SerializeField] private Transform _explosionPoint;
    [SerializeField] private float _distanceToDisappear;

    private bool _rotating = false;
    private Transform _target;
    private AudioSource _source;

    public void Init(Transform target)
    {
        _target = target;
    }

    public void SetRotating()
    {
        _rotating = true;
        GetComponent<BoxCollider>().size = _rotationColliderSize;
    }

    private void Awake()
    {
        StartCoroutine(LateDestroy());
        _source = GetComponent<AudioSource>();
    }

    private void FixedUpdate()
    {
        Move();
        float distanceAfterTarget = _target.position.z - transform.position.z;
        if (transform.position.z < _target.position.z)
            _source.volume = 1 - Mathf.Lerp(0, _distanceToDisappear, distanceAfterTarget / _distanceToDisappear) / _distanceToDisappear;
        if (distanceAfterTarget >= _distanceToDisappear)
            Destroy(gameObject);
        if (_rotating)
            _model.transform.localRotation *= Quaternion.Euler(_rotationSpeed, 0f, 0f);
    }

    private void Move()
    {
        var offset = _forwardSpeed * Time.fixedDeltaTime;
        transform.Translate(0, 0, offset);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explosion, _explosionPoint.position, Quaternion.identity);
        Destroy(gameObject);
    }

    private IEnumerator LateDestroy()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }
}

