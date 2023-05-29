using Assets.Scripts;
using System;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthView : MonoBehaviour
{
    [SerializeField] private Image[] _heathIcons;
    [SerializeField] private Color _defaultHealthColor;
    [SerializeField] private Color _lostHealthColor;
    [SerializeField] private PlayerCharacter _character;

    private Health _health => _character.Health;

    private void Awake()
    {
        if (_health.MaxValue != _heathIcons.Length)
            throw new InvalidOperationException(nameof(_heathIcons.Length));

        _health.Changed += OnHealthChanged;
    }

    private void OnDestroy()
    {
        _health.Changed -= OnHealthChanged;
    }

    private void OnHealthChanged()
    {
        int i = 1;
        foreach (var icon in _heathIcons)
        {
            icon.color = i <= _health.Value ? _defaultHealthColor : _lostHealthColor;
            i++;
        }
    }
}
