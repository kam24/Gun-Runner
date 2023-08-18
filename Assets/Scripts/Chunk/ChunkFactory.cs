using Assets.Scripts;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;

public class ChunkFactory : MonoBehaviour
{
    [SerializeField] private Transform _firstChunkStart;
    [SerializeField] private AssetReferenceGameObject[] _starterChunks;
    [SerializeField] private AssetReferenceGameObject[] _mediumChunks;
    [SerializeField] private AssetReferenceGameObject[] _droneAttackChunks;
    [SerializeField][Min(0)] private float _chunkSpawnDistance;
    [SerializeField][Min(0)] private float _lastChunkDestructionDistance;

    public event Action ChunkPassed;

    private AssetReferenceGameObject[] _chunksToSpawn;
    private Stack<AssetReferenceGameObject> _chunksPool;
    private Queue<ChunkLoader> _spawnedChunks;
    private ChunkLoader _currentChunk => _spawnedChunks.Peek();
    private ChunkLoader _lastChunk;
    private Transform _playerTransform;
    private bool canDestroyLastChunk;
    private bool canSpawnNextChunk;

    public void Init(Transform playerPosition)
    {
        _spawnedChunks = new Queue<ChunkLoader>();

        _playerTransform = playerPosition;

        SetSpawningStartChunks();
        Spawn(_chunksPool.Pop());

        SetSpawningMediumChunks();

        canDestroyLastChunk = true;
        canSpawnNextChunk = true;

        enabled = true;
    }

    public void SetSpawningStartChunks()
    {
        SetSpawningChunks(_starterChunks);
    }

    public void SetSpawningMediumChunks()
    {
        SetSpawningChunks(_mediumChunks);
    }

    public void SetSpawningDroneAttackChunks()
    {
        SetSpawningChunks(_droneAttackChunks);
    }

    private void SetSpawningChunks(AssetReferenceGameObject[] chunks)
    {
        _chunksToSpawn = chunks;
        ShuffleChunksPool();
    }

    private void FixedUpdate()
    {
        if (_spawnedChunks.Count == 0)
            return;

        float playerDistance = _playerTransform.position.z - _currentChunk.Chunk.Start.z;
        TryDestroyLastChunk(playerDistance);
        TrySpawnNextChunk(playerDistance);

        if (_playerTransform.position.z > _currentChunk.Chunk.End.z)
        {
            _lastChunk = _currentChunk;
            _spawnedChunks.Dequeue();
            canDestroyLastChunk = true;
            canSpawnNextChunk = true;
            ChunkPassed?.Invoke();
        }
    }

    private void OnDisable()
    {
        _lastChunk.UnloadInternal();
        for (int i = 0; i < _spawnedChunks.Count; i++)
            _spawnedChunks.Dequeue().UnloadInternal();
    }

    private void TryDestroyLastChunk(float playerDistance)
    {
        if (canDestroyLastChunk && playerDistance > _lastChunkDestructionDistance)
        {
            canDestroyLastChunk = false;
            _lastChunk.UnloadInternal();
        }
    }
    
    private void TrySpawnNextChunk(float playerDistance)
    {
        if (canSpawnNextChunk && playerDistance > _chunkSpawnDistance)
        {
            canSpawnNextChunk = false;

            var chunk = _chunksPool.Pop();
            Spawn(chunk);

            if (_chunksPool.Count == 0)
                _chunksPool = new Stack<AssetReferenceGameObject>(Shuffle(_chunksToSpawn));
        }
    }

    private async void Spawn(AssetReferenceGameObject chunk)
    {
        var nextChunk = new ChunkLoader();
        await nextChunk.LoadInternal(chunk);
        var currentChunkPosition = _spawnedChunks.Count > 0 ? _currentChunk.Chunk.End : _firstChunkStart.position;
        nextChunk.Chunk.transform.position -= nextChunk.Chunk.Start - currentChunkPosition;
        nextChunk.Chunk.Init();
        _spawnedChunks.Enqueue(nextChunk);
    }

    private void ShuffleChunksPool()
    {
        _chunksPool = new Stack<AssetReferenceGameObject>(Shuffle(_chunksToSpawn));
    }

    private AssetReferenceGameObject[] Shuffle(AssetReferenceGameObject[] chunks)
    {
        var random = new System.Random();
        for (int i = chunks.Length - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);

            (chunks[i], chunks[j]) = (chunks[j], chunks[i]);
        }

        return chunks;
    }
}