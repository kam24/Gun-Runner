using Cinemachine;
using UnityEditor;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class RestartButton : MonoBehaviour
{
    [SerializeField] private string _scenePathToLoad;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(RestartGame);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(RestartGame);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(_scenePathToLoad);
    }
}
