using System;
using System.Collections;
using UnityEngine;

public class UIController : MonoBehaviour
{
    [SerializeField] private Canvas _playerUI;
    [SerializeField] private Menu _menuUI;
    [SerializeField] private float _timeToShowMenu;

    public void Init(PlayerCharacter playerCharacter)
    {
        playerCharacter.Killed += SetGameOver;

        enabled = true;
    }

    private void SetGameOver()
    {
        StartCoroutine(ExecuteAfterTime(_timeToShowMenu, () =>
        {
            _playerUI.enabled = false;
            _menuUI.ShowGameOver();
        }));
    }

    private IEnumerator ExecuteAfterTime(float time, Action callBack)
    {
        yield return new WaitForSeconds(time);
        callBack?.Invoke();
    }
}