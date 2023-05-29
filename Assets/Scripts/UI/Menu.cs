using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private RectTransform _startButton;
    [SerializeField] private RectTransform _gameOver;

    public void ShowGameOver()
    {
        gameObject.SetActive(true);
        _startButton.gameObject.SetActive(false);
        _gameOver.gameObject.SetActive(true);
    }
}
