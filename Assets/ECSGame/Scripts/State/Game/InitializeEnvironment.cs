using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.AssetLoader;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.State.Game
{
    public class InitializeEnvironment : ILoadable
    {
        private IAssetLoader _assetLoader;

        public InitializeEnvironment(IAssetLoader assetLoader)
        {
            _assetLoader = assetLoader;
        }

        public async UniTask Load()
        {
            await _assetLoader.Load<MainEnvironment>("MainEnvironment");
        }
    }
}