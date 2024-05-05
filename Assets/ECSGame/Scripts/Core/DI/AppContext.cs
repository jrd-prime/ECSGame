using System;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.AssetLoader;
using ECSGame.Scripts.Core.Config;
using ECSGame.Scripts.Core.Config.Providers;
using ECSGame.Scripts.Core.DataBase;
using ECSGame.Scripts.Core.DI.Factory;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;
using ECSGame.Scripts.TestDB;
using UnityEngine;

namespace ECSGame.Scripts.Core.DI
{
    /// <summary>
    /// Init services and inject dependencies
    /// </summary>
    public class AppContext : MonoBehaviour, IAppContext
    {
        [SerializeField] public GameConfigScriptable _config;

        public LoadingScreenProvider LoadingScreenProvider { get; private set; }

        public IAssetLoader AssetLoader { get; private set; }
        public static AppContext Instance { get; private set; }
        public IMyContainer Container { get; private set; }
        public IDataBase DataBase { get; private set; }

        public Providers Provider { get; private set; }
        private ConfigManager _configManager;
        private Loader _loader;

        private AppContext()
        {
        }

        private void Awake()
        {
            Instance = this;
            Provider = Providers.Init();
            _configManager = ConfigManager.Instance;
            _configManager.Init(_config);

            DontDestroyOnLoad(this);
        }

        public async UniTask Init(Loader loader)
        {
            _loader = loader;

            var containerInit = Provider
                .Get<ContainerProvider>()
                .GetImplInstance<IContainerInitializer>();
            Container = containerInit.GetContainer();
            var serviceFactory = Provider
                .Get<ServiceFactoryProvider>()
                .GetImplInstance<IServiceFactory>();
            var containerFactory = Provider
                .Get<ContainerFactoryProvider>()
                .GetImplInstance<IMyContainerFactory>();
            DataBase = Provider
                .Get<DataBaseProvider>()
                .GetImplInstance<IDataBase>();
            AssetLoader = Provider
                .Get<AssetLoaderProvider>()
                .GetImplInstance<IAssetLoader>();

            LoadingScreenProvider = new LoadingScreenProvider();

            // Loading queue
            loader.AddToLoadingQueue(_configManager, 2000);
            loader.AddToLoadingQueue(containerInit, 2000);
            loader.AddToLoadingQueue(containerFactory, 2000);
            loader.AddToLoadingQueue(serviceFactory, 2000);


            loader.AddToLoadingQueue(DataBase, 2000);
            loader.AddToLoadingQueue(AssetLoader, 2000);

            await UniTask.CompletedTask;
        }

        // #if !UNITY_INCLUDE_TESTS 
//         private void OnValidate()
//         {
//             if (_gameConfig == null) throw new Exception($"Game config not set to {gameObject.name}!");
//         }
// #endif
    }
}