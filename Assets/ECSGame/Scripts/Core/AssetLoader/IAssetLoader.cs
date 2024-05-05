using Cysharp.Threading.Tasks;
using ECSGame.Scripts.State.Loading;
using UnityEngine;

namespace ECSGame.Scripts.Core.AssetLoader
{
    public interface IAssetLoader : ILoadable
    {
        string ILoadable.Description => "Asset Loader";

        public UniTask<T> Load<T>(string assetName, Transform parent = null);
        public void Unload();
    }
}