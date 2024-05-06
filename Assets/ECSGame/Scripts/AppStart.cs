using System.Collections.Generic;
using ECSGame.Scripts.Core.AssetLoader;
using ECSGame.Scripts.Core.DI;
using ECSGame.Scripts.State.Game;
using ECSGame.Scripts.State.Loading;
using ECSGame.Scripts.Utils;
using UnityEngine;
using System;
using System.Linq;
using System.Reflection;
using Cysharp.Threading.Tasks;
using UnityEngine.AddressableAssets;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;
using AppContext = ECSGame.Scripts.Core.DI.AppContext;
using Scene = ECSGame.Scripts.Core.Config.Const.Scene;

namespace ECSGame.Scripts
{
    public class AppStart : MonoBehaviour
    {
        [SerializeField] private GameObject _appContextHolderGo;

        private AppContext _context;
        public Loader Loader { get; private set; }

        private void Awake()
        {
            _context = _appContextHolderGo.GetComponent<AppContext>();
            Loader = new Loader();
        }

        private async void Start()
        {
            await _context.Init(Loader);

            await _context.LoadingScreenProvider.LoadAndDestroy(Loader);

            var gameScene = Addressables.LoadSceneAsync(Scene.Game, LoadSceneMode.Additive);
            await gameScene.Task;

            var tempActiveScene = SceneManager.GetActiveScene();

            SceneManager.SetActiveScene(gameScene.Result.Scene);

            SceneManager.UnloadSceneAsync(tempActiveScene);
            JLog.Msg($"(Services initialization FINISHED...");
            Debug.LogWarning("START THE GAME");

            await UniTask.Delay(3000);
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