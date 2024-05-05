using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.AssetLoader;

namespace ECSGame.Scripts.State.Loading
{
    public class LoadingScreenProvider : LocalAssetLoader
    {
        public async UniTask LoadAndDestroy(Loader loader)
        {
            var loadingScreen = await Load<LoadingScreenView>("LoadingScreen");

            loadingScreen._steps = loader.LoadingQueue.Count;

            await loadingScreen.Load(loader);
            Unload();
        }
    }
}