using System;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;

namespace Assets.Scripts
{
    public class ChunkLoader
    {
        public Chunk Chunk { get; private set; }

        private GameObject _cachedObject;

        public async Task<Chunk> LoadInternal(AssetReferenceGameObject chunk)
        {
            var handle = Addressables.InstantiateAsync(chunk);
            _cachedObject = await handle.Task;
            if (_cachedObject.TryGetComponent(out Chunk component) == false)
                throw new NullReferenceException($"Object of type {typeof(Chunk)} is null on " +
                                                 "attempt to load it from addressables");
            Chunk = component;
            return component;
        }

        public void UnloadInternal()
        {
            if (_cachedObject == null)
                return;
            _cachedObject.SetActive(false);
            Addressables.ReleaseInstance(_cachedObject);
            _cachedObject = null;
            Chunk = null;
        }
    }
}
