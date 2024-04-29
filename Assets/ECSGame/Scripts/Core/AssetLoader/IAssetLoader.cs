using Cysharp.Threading.Tasks;
using UnityEngine;

namespace ECSGame.Scripts.Core.AssetLoader
{
    public interface IAssetLoader
    {
        public UniTask<T> Load<T>(string assetName, Transform parent = null);
        public void Unload();
    }
}