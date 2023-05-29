using System;
using System.Collections.Generic;
using UnityEngine;

public class ChunkFactory : MonoBehaviour
{
    [SerializeField] private Transform _firstChunkStart;
    [SerializeField] private Chunk[] _starterChunks;
    [SerializeField] private Chunk[] _mediumChunks;
    [SerializeField] private Chunk[] _droneAttackChunks;
    [SerializeField][Min(0)] private float _chunkSpawnDistance;
    [SerializeField][Min(0)] private float _lastChunkDestructionDistance;

    public event Action ChunkPassed;

    private Chunk[] _chunksToSpawn;
    private Stack<Chunk> _chunksPool;
    private Queue<Chunk> _spawnedChunks;
    private Chunk _currentChunk => _spawnedChunks.Peek();
    private Chunk _lastChunk;
    private Transform _playerTransform;
    private bool canDestroyLastChunk;
    private bool canSpawnNextChunk;

    public void Init(Transform playerPosition)
    {
        _playerTransform = playerPosition;
        _chunksPool = new Stack<Chunk>(Shuffle(_starterChunks));
        _spawnedChunks = new Queue<Chunk>();
        Spawn(_chunksPool.Pop());
        SetSpawningMediumChunks();

        canDestroyLastChunk = true;
        canSpawnNextChunk = true;

        enabled = true;
    }

    public void SetSpawningMediumChunks()
    {
        _chunksToSpawn = _mediumChunks;
        ShuffleChunksPool();
    }

    public void SetSpawningDroneAttackChunks()
    {
        _chunksToSpawn = _droneAttackChunks;
        ShuffleChunksPool();
    }

    private void FixedUpdate()
    {
        float playerDistance = _playerTransform.position.z - _currentChunk.Start.z;
        TryDestroyLastChunk(playerDistance);
        TrySpawnNextChunk(playerDistance);

        if (_playerTransform.position.z > _currentChunk.End.z)
        {
            _lastChunk = _currentChunk;
            _spawnedChunks.Dequeue();
            canDestroyLastChunk = true;
            canSpawnNextChunk = true;
            ChunkPassed?.Invoke();
        }
    }

    private void TryDestroyLastChunk(float playerDistance)
    {
        if (canDestroyLastChunk && playerDistance > _lastChunkDestructionDistance)
        {
            canDestroyLastChunk = false;
            Destroy(_lastChunk?.gameObject);
        }
    }
    
    private void TrySpawnNextChunk(float playerDistance)
    {
        if (canSpawnNextChunk && playerDistance > _chunkSpawnDistance)
        {
            canSpawnNextChunk = false;
            var chunk = _chunksPool.Pop();

            if (chunk == _lastChunk)
                chunk = _chunksPool.Pop();

            Spawn(chunk);

            if (_chunksPool.Count == 0)
                _chunksPool = new Stack<Chunk>(Shuffle(_chunksToSpawn));
        }
    }

    private void Spawn(Chunk chunk)
    {
        var nextChunk = Instantiate(chunk);
        var currentChunkPosition = _spawnedChunks.Count > 0 ? _currentChunk.End : _firstChunkStart.position;
        nextChunk.transform.position -= nextChunk.Start - currentChunkPosition;
        _spawnedChunks.Enqueue(nextChunk);
    }

    private void ShuffleChunksPool()
    {
        _chunksPool = new Stack<Chunk>(Shuffle(_chunksToSpawn));
    }

    private Chunk[] Shuffle(Chunk[] chunks)
    {
        var random = new System.Random();
        for (int i = chunks.Length - 1; i >= 1; i--)
        {
            int j = random.Next(i + 1);

            (chunks[i], chunks[j]) = (chunks[j], chunks[i]);
        }

        return chunks;
    }

    public void SpawnFinish()
    {
        //
    }
}