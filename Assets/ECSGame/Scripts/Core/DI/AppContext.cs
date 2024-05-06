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
        public ConfigManager ConfigManager { get; private set; }
        private ProvidersFactory _providersFactory;
        
       

        private AppContext()
        {
        }

        private async void Awake()
        {
            Instance = this;

            ConfigManager = await ConfigManager.Instance.Init(_config);

            // Init providers based on configuration
            _providersFactory = new ProvidersFactory(_config);

            LoadingScreenProvider = new LoadingScreenProvider();

            DontDestroyOnLoad(this);
        }

        public async UniTask Init(Loader loader)
        {
            if (loader == null) throw new ArgumentNullException();
            if (_providersFactory == null) throw new NullReferenceException("Providers factory is null!");

            IContainerInitializer containerInitializer = _providersFactory.GetProvidedInstance<IContainerInitializer>();
            IContainerFactory containerFactory = _providersFactory.GetProvidedInstance<IContainerFactory>();
            IServiceFactory serviceFactory = _providersFactory.GetProvidedInstance<IServiceFactory>();

            Container = containerInitializer!.GetContainer();
            DataBase = _providersFactory.GetProvidedInstance<IDataBase>();
            AssetLoader = _providersFactory.GetProvidedInstance<IAssetLoader>();


            // Loading queue
            loader.AddToQueue(ConfigManager, 1000);
            loader.AddToQueue(containerInitializer, 2000);
            loader.AddToQueue(containerFactory, 2000);
            loader.AddToQueue(serviceFactory, 2000);
            loader.AddToQueue(DataBase, 2000);
            loader.AddToQueue(AssetLoader, 2000);

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