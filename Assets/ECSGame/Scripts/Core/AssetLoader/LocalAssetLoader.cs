using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Debug = UnityEngine.Debug;

namespace ECSGame.Scripts.Core.AssetLoader
{
    public class LocalAssetLoader : IAssetLoader
    {
        protected GameObject CachedObj;

        public async UniTask<T> Load<T>(string assetName, Transform parent = null)
        {
            var handle = Addressables.InstantiateAsync(assetName, parent);
            CachedObj = await handle.Task;
            if (CachedObj.TryGetComponent(out T component) == false)
                throw new NullReferenceException($"Object of type {typeof(T)} is null on " +
                                                 "attempt to load it from addressables");
            return component;
        }


        public void Unload()
        {
            if (CachedObj == null)
                return;
            CachedObj.SetActive(false);
            Addressables.ReleaseInstance(CachedObj);
            CachedObj = null;
        }
    }
}