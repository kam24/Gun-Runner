using Assets.InputSystem;
using Assets.Scripts;
using UnityEngine;

public class Root : MonoBehaviour
{
    [SerializeField] private CinemachineCameraController _cameraController;
    [SerializeField] private ChunkFactory _chunkFactory;
    [SerializeField] private EnemySpawner _enemySpawner;
    [SerializeField] private LevelController _levelController;
    [SerializeField] private MusicController _musicController;
    [SerializeField] private UIController _uiController;
    [SerializeField] private PlayerCharacter _character;
    [SerializeField] private CinemachineShake _cinemachineShake;
    [SerializeField] private PoolService _poolService;
    [SerializeField] private float mediumLineX;
    [SerializeField] private float sideLineOffset;
    [SerializeField] private int _initialPlayerLine = 1;

    public static CollectablesController CollectablesManager { get; private set; }
    public static SubmachineGun SubmachineGun { get; private set; }
    public static RunLine RunLine { get; private set; }
    public static PlayerCharacter PlayerCharacter { get; private set; }
    public static ExplosionRegister ExplosionRegister { get; private set; }

    private PlayerInputRouter _router;
    private RunLine _runLine;

    public void Init()
    {
        _router = new PlayerInputRouter(_character);
        SubmachineGun = new();
        PlayerCharacter = _character;
        InitPlayer();
        _cameraController.Init(_character);
        _cinemachineShake.Init(_character);
        ExplosionRegister = new(_character, _cinemachineShake);
        CollectablesManager = new(SubmachineGun, _character);
        _enemySpawner.Init(_character);
        _chunkFactory.Init(_character.transform);
        _levelController.Init(_enemySpawner, _chunkFactory);
        _musicController.Init(_enemySpawner);
        _uiController.Init(_character);
        _poolService.Init();

        enabled = true;
    }

    private void OnEnable()
    {
        _router.OnEnable();
    }

    private void OnDisable()
    {
        _router.OnDisable();
    }

    private void InitPlayer()
    {
        _runLine = new RunLine(sideLineOffset, mediumLineX, _initialPlayerLine);
        RunLine = _runLine;
        Vector3 newPosition = new(_runLine.GetCurrent(), _character.transform.position.y, _character.transform.position.z);
        _character.transform.position = newPosition;
        _character.Init(_runLine, new(3));
    }
}
