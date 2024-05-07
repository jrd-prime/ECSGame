using ECSGame.Scripts.State.Loading;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using Scene = ECSGame.Scripts.Core.Config.Const.Scene;

namespace ECSGame.Scripts
{
    public class AppStart : MonoBehaviour
    {
        [SerializeField] private GameObject _appContextHolderGo;

        private AppContext _context;

        private void Awake() => _context = _appContextHolderGo.GetComponent<AppContext>();

        private async void Start()
        {
            Loader loader = new();
            // Init services for load
            await _context.Init(loader);
            // Show loading screen and load services
            await _context.LoadingScreenProvider.LoadAndDestroy(loader);
            // Load env scene and start the game
            await LoadGameScene();
        }

        private async UniTask LoadGameScene()
        {
            var gameScene = Addressables.LoadSceneAsync(Scene.Game, LoadSceneMode.Additive);
            await gameScene.Task;

            var tempActiveScene = SceneManager.GetActiveScene();
            SceneManager.SetActiveScene(gameScene.Result.Scene);
            await SceneManager.UnloadSceneAsync(tempActiveScene);
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            if (_appContextHolderGo == null)
                throw new NullReferenceException($"AppContextHolder not set to {gameObject.name}!");
        }
#endif
    }
}