using System;
using UnityEngine;

public class LevelController : MonoBehaviour
{
    [SerializeField] private ushort _countOfDefeatedDronesToFinish;

    private EnemySpawner _enemySpawner;
    private ChunkFactory _chunkFactory;
    private Stage _stage;

    public void Init(EnemySpawner enemySpawner, ChunkFactory chunkFactory)
    {
        _enemySpawner = enemySpawner;
        _chunkFactory = chunkFactory;

        enabled = true;
    }

    private void Awake()
    {
        _stage = new Stage();
        OnStageChanged();
    }

    private void OnEnable()
    {
        _stage.Changed += OnStageChanged;
        _chunkFactory.ChunkPassed += OnChunkPassed;
        _enemySpawner.DroneDying += OnDroneDefeated;
    }

    private void OnDisable()
    {
        _stage.Changed -= OnStageChanged;
        _chunkFactory.ChunkPassed -= OnChunkPassed;
        _enemySpawner.DroneDying -= OnDroneDefeated;
    }

    private void OnChunkPassed()
    {
        _stage.GoNext();
    }

    private void OnStageChanged()
    {
        if (_stage.Number == 3)
        {
            _chunkFactory.SetSpawningDroneAttackChunks();
        }

        if (_stage.Number == 4)
        {
            _enemySpawner.SpawnShootingDrone();
        }
    }

    private void OnDroneDefeated()
    {
        _stage.Reset();
        _chunkFactory.SetSpawningMediumChunks();
    }
}

public class Stage
{
    public event Action Changed;

    public int Number { get; private set; }

    public Stage() => Number = 1;

    public void GoNext()
    {
        Number++;
        Changed?.Invoke();
    }

    public void Reset()
    {
        Number = 1;
        Changed?.Invoke();
    }
}

