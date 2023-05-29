using UnityEngine;
using static UnityEngine.ParticleSystem;

[RequireComponent(typeof(ParticleSystem))]
public class ParticleDestroyer : MonoBehaviour
{
    private ParticleSystem _particleSystem;

    private void Start()
    {
        _particleSystem = GetComponent<ParticleSystem>();
    }

    private void Update()
    {
        if (_particleSystem.IsAlive() == false || (_particleSystem.main.maxParticles == 0 && _particleSystem.particleCount == 0))
            Destroy(gameObject);
    }
}
