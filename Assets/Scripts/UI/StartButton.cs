using Cinemachine;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class StartButton : MonoBehaviour
{
    [SerializeField] private Root _root;
    [SerializeField] private Canvas _menuUI;
    [SerializeField] private Canvas _UIToActivate;
    [SerializeField] private CinemachineVirtualCamera _cameraToActivate;

    private Button _button;

    private void Awake()
    {
        _button = GetComponent<Button>();
        _button.onClick.AddListener(StartGame);
    }

    private void OnDestroy()
    {
        _button.onClick.RemoveListener(StartGame);
    }

    private void StartGame()
    {
        _root.Init();
        _menuUI.gameObject.SetActive(false);
        _cameraToActivate.gameObject.SetActive(true);
        _UIToActivate.gameObject.SetActive(true);
    }
}
