using Assets.Scripts;
using TMPro;
using UnityEngine;

[RequireComponent (typeof(TextMeshProUGUI))]
public class BulletCounterView : MonoBehaviour
{
    private TextMeshProUGUI _textMeshPro;
    private SubmachineGun Gun => Root.SubmachineGun;

    private void Awake()
    {
        _textMeshPro = GetComponent<TextMeshProUGUI>();
        Gun.BulletsChanged += OnBulletsCountChanged;
        OnBulletsCountChanged();
    }

    private void OnDestroy()
    {
        Gun.BulletsChanged -= OnBulletsCountChanged;
    }

    private void OnBulletsCountChanged()
    {
        _textMeshPro.text = Gun.Bullets.ToString(); 
    }
}
