using System.Collections.Generic;
using ECSGame.Scripts.Core.AssetLoader;
using ECSGame.Scripts.Core.DI;
using ECSGame.Scripts.State.Game;
using ECSGame.Scripts.State.Loading;
using ECSGame.Scripts.Utils;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.SceneManagement;
using AppContext = ECSGame.Scripts.Core.DI.AppContext;

namespace ECSGame.Scripts
{
    public class AppStart : MonoBehaviour
    {
        [SerializeField] private GameObject _appContextHolderGo;

        private AppContext _context;
        private IAssetLoader _assetLoader;
        private LoadingScreenProvider _loadingScreenProvider;

        private async void Awake()
        {
            _context = _appContextHolderGo.GetComponent<AppContext>();
            _loadingScreenProvider = new LoadingScreenProvider();
            _assetLoader = new LocalAssetLoader();
        }

        private async void Start()
        {
            var s = "GameScene";

            var b = Addressables.LoadSceneAsync(s, LoadSceneMode.Additive);
            await b.Task;

            var se = SceneManager.GetActiveScene();


            _context.Init();

            var loading = new Queue<ILoadable>();
            loading.Enqueue(_context.ConfigManager);
            loading.Enqueue(_context.ContainerProvider);
            loading.Enqueue(new InitializeDI());
            loading.Enqueue(new InitializeEnvironment(_assetLoader));

            await _loadingScreenProvider.LoadAndDestroy(loading);
            SceneManager.SetActiveScene(b.Result.Scene);

            SceneManager.UnloadSceneAsync(se);

            // if (a.isDone)
            // {
            //     SceneManager.UnloadScene("LoaderScene");
            // }


            JLog.Msg($"(Services initialization FINISHED...");
            Debug.LogWarning("START THE GAME");
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