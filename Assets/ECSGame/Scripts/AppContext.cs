using System;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.AssetLoader;
using ECSGame.Scripts.Core.Config;
using ECSGame.Scripts.Core.Config.Providers;
using ECSGame.Scripts.Core.DataBase;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;
using UnityEngine;

namespace ECSGame.Scripts
{
    /// <summary>
    /// Init services and inject dependencies
    /// </summary>
    public class AppContext : MonoBehaviour, IAppContext
    {
        [SerializeField] public GameConfigScriptable _config;

        public LoadingScreenProvider LoadingScreenProvider { get; private set; }
        public IAssetLoader AssetLoader { get; private set; }
        public IMyContainer Container { get; private set; }
        public IDataBase DataBase { get; private set; }
        public ConfigManager ConfigManager { get; private set; }
        public ProvidersFactory providersFactory { get; private set; }
        public static AppContext Instance { get; private set; }

        private AppContext()
        {
        }

        private async void Awake()
        {
            Instance = this;
            ConfigManager = await ConfigManager.Instance.Init(_config);
            providersFactory = new ProvidersFactory(_config);
            LoadingScreenProvider = new LoadingScreenProvider();
            DontDestroyOnLoad(this);
        }

        public async UniTask Init(Loader loader)
        {
            if (loader == null) throw new ArgumentNullException();
            if (providersFactory == null) throw new NullReferenceException("Providers factory is null!");

            // Get services instances
            IContainerInitializer containerInitializer = providersFactory.GetProvidedInstance<IContainerInitializer>();
            IContainerFactory containerFactory = providersFactory.GetProvidedInstance<IContainerFactory>();
            DataBase = providersFactory.GetProvidedInstance<IDataBase>();
            AssetLoader = providersFactory.GetProvidedInstance<IAssetLoader>();
            
            Container = containerInitializer!.GetContainer();

            // Loading queue
            loader.AddToQueue(ConfigManager, 1000);
            loader.AddToQueue(containerInitializer, 2000);
            loader.AddToQueue(containerFactory, 2000);
            loader.AddToQueue(DataBase, 2000);
            loader.AddToQueue(AssetLoader, 2000);

            await UniTask.CompletedTask;
        }
    }
}