using Assets.Scripts;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent (typeof(Slider))]
public class DroneHealthView: MonoBehaviour
{
    [SerializeField] private ShootingDrone _drone;

    private Health _health => _drone.Health;
    private Slider _slider;

    private void Start()
    {
        _slider = GetComponent<Slider>();
        _health.Changed += OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        _slider.value = _health.Value / _health.MaxValue;
    }
}

