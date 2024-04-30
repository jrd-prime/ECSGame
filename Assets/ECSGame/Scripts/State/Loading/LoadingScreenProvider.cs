﻿using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.AssetLoader;

namespace ECSGame.Scripts.State.Loading
{
    public class LoadingScreenProvider : LocalAssetLoader
    {
        public async UniTask LoadAndDestroy(Queue<ILoadable> loadable)
        {
            var loadingScreen = await Load<LoadingScreen>("LoadingScreen");
            await loadingScreen.Load(loadable);
            Unload();
        }
    }
}