
using UnityEngine.AddressableAssets;

namespace Assets.Scripts
{
    public class AssetReferenceChunk : AssetReferenceT<Chunk>
    {
        public AssetReferenceChunk(string guid) : base(guid)
        {
        }
    }
}
