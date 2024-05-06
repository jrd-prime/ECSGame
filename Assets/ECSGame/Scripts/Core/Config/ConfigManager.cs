using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.Config.Providers;
using ECSGame.Scripts.Core.DataBase;
using ECSGame.Scripts.Core.DI;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;
using ECSGame.Scripts.TestConfig;
using ECSGame.Scripts.TestDB;
using JetBrains.Annotations;
using UnityEngine;
using AppContext = ECSGame.Scripts.Core.DI.AppContext;
using Cache = ECSGame.Scripts.Core.DI.Cache;

namespace ECSGame.Scripts.Core.Config
{
    public class ConfigManager : ILoadable
    {
        public string Description => "Config Manager";
        public static GameConfigScriptable GameConfig { get; private set; }
        private GameConfigScriptable _gameConfig;

        private static ConfigManager _instance;
        private AppContext _context;
        public static ConfigManager Instance => _instance ??= new ConfigManager();


        private static Dictionary<Type, object> _cache;
        private static Dictionary<Type, Type> _implTypeCache;

        private static Type _databaseType;
        private static Type _serviceFactoryType;
        private static bool _isInit;


        private ConfigManager()
        {
        }


        public async UniTask<ConfigManager> Init(GameConfigScriptable gameConfig)
        {
            if (_isInit) return await UniTask.FromResult(Instance);

            _context = AppContext.Instance;

            GameConfig = gameConfig;


            _cache = new Dictionary<Type, object>();
            _isInit = true;
            return await UniTask.FromResult(Instance);
        }

        public void SetConfig<T>(ref IConfiguration config) where T : class, IConfiguration
        {
            if (config == null) throw new ArgumentNullException($"Config is null!");

            _cache.TryAdd(typeof(T), config);
        }

        [CanBeNull]
        public T GetConfiguration<T>() where T : class, IConfiguration
        {
            if (!_cache.ContainsKey(typeof(T))) return null;

            return _cache[typeof(T)] as T;
        }

        public async UniTask Load(Action<ILoadable> action)
        {
            if (!_isInit) throw new Exception("ConfigManager not init. Before load -> Init();");

            action.Invoke(this);


            var serviceFactoryType =
                new ServiceFactoryProvider().GetImplType(GameConfig._containerServiceFactory);
            var containerFactoryType = new ContainerFactoryProvider().GetImplType(GameConfig._containerFactory);
            // var assetLoaderType =
            //     AssetLoaderProvider.Instance.GetProviderType(GameConfig._assetLoader);


            IConfiguration bindsConfiguration = new TestBinds();

            IConfiguration containerConfig = new DefaultContainerConfig
            {
                Impl = new Dictionary<Type, Type>
                {
                    { typeof(IMyContainer), typeof(MyContainer) },
                    { typeof(IBinder), typeof(Binder) },
                    { typeof(ICache), typeof(Cache) },
                    { typeof(IInjector), typeof(Injector) },
                    { typeof(IServiceFactory), serviceFactoryType },
                    { typeof(IDataBase), _databaseType },
                    { typeof(IContainerFactory), containerFactoryType }
                }
            };

            SetConfig<IContainerConfig>(ref containerConfig);
            SetConfig<IBindsConfiguration>(ref bindsConfiguration);

            await UniTask.Delay(3000);
        }

        public Type GetImplType<T>() where T : class
        {
            return _implTypeCache[typeof(T)];
        }
    }
}