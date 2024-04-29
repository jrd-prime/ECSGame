using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.DI;
using ECSGame.Scripts.Core.DI.Factory;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;
using ECSGame.Scripts.TestConfig;
using JetBrains.Annotations;
using UnityEngine;
using Cache = ECSGame.Scripts.Core.DI.Cache;

namespace ECSGame.Scripts.Core.Config
{
    public class ConfigManager : ILoadable
    {
        public GameConfigScriptable GameConfig { get; private set; }

        private static Dictionary<Type, object> _cache;

        public ConfigManager(GameConfigScriptable gameConfig)
        {
            GameConfig = gameConfig;
            _cache = new Dictionary<Type, object>();
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

        public void SetGameConfig(GameConfigScriptable gameConfig) => GameConfig = gameConfig;

        public async UniTask Load()
        {
            Debug.LogWarning("Loading " + this);
            Type serviceFactoryType =
                ServiceFactoryManager.I.GetFactoryType(GameConfig._serviceFactory);
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
                    { typeof(IMyContainerFactory), typeof(DefaultContainerFactory) }
                }
            };

            SetConfig<IContainerConfig>(ref containerConfig);
            SetConfig<IBindsConfiguration>(ref bindsConfiguration);

            await UniTask.Delay(3000);
            Debug.LogWarning("Loaded " + this);
        }
    }
}