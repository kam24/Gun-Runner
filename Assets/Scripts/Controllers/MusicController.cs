using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicController : MonoBehaviour
{
    [SerializeField] private AudioClip _common;
    [SerializeField] private AudioClip _droneAttack;

    private EnemySpawner _enemySpawner;
    private AudioSource _audioSource;

    public void Init(EnemySpawner enemySpawner)
    {
        _enemySpawner = enemySpawner;
        _audioSource = GetComponent<AudioSource>();
        SetAudioClip(_common);

        enabled = true;
    }

    private void OnEnable()
    {
        _enemySpawner.DroneSpawned += OnDroneSpawned;
        _enemySpawner.DroneDying += OnDroneDying;
    }

    private void OnDisable()
    {
        _enemySpawner.DroneSpawned -= OnDroneSpawned;
        _enemySpawner.DroneDying -= OnDroneDying;
    }

    private void OnDroneDying()
    {
        SetAudioClip(_common);
    }

    private void OnDroneSpawned()
    {
        SetAudioClip(_droneAttack);
    }

    private void SetAudioClip(AudioClip audioClip)
    {
        _audioSource.clip = audioClip;
        _audioSource.Play();
    }
}