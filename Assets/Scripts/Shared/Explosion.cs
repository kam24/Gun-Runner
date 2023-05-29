using System.Collections;
using UnityEngine;
using UnityEngine.Scripting;

[RequireComponent(typeof(SphereCollider))]
public class Explosion : MonoBehaviour
{
    [SerializeField] private DamageOrigin _damageOrigin;
    [SerializeField] private float _lifetime;
    [SerializeField] private GameObject _fx;

    private void Awake()
    {
        Root.ExplosionRegister.Register(transform.position, _damageOrigin);
        var fx = Instantiate(_fx, transform.position, Quaternion.identity);
        fx.SetActive(true);
        StartCoroutine(LateDestroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent(out Damagable damagable))
            damagable.ApplyDamage(_damageOrigin);
    }

    private IEnumerator LateDestroy()
    {
        yield return new WaitForSeconds(_lifetime);
        Destroy(gameObject);
    }
}
