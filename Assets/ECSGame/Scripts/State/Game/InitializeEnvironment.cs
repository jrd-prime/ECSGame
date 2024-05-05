using System;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.AssetLoader;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.State.Game
{
    public class InitializeEnvironment : ILoadable
    {
        public string Description => "InitializeEnvironment";
        private readonly IAssetLoader _assetLoader;

        public InitializeEnvironment(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public async UniTask Load(Action<ILoadable> action)
        {
            action.Invoke(this);
            await _assetLoader.Load<MainEnvironment>("MainEnvironment");
            await UniTask.Delay(1111);
        }
    }
}