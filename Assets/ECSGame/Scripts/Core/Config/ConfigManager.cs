using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using ECSGame.Scripts.Core.Config.Interface;
using ECSGame.Scripts.Core.DI.Config;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.Core.Config
{
    public class ConfigManager : ILoadable
    {
        public string Description => "Config Manager";
        public static ConfigManager Instance => _instance ??= new ConfigManager();

        private static ConfigManager _instance;
        private readonly Dictionary<Type, object> _cache = new();

        private ConfigManager()
        {
        }

        //TODO refact / config to cache through providers
        public async UniTask<ConfigManager> Init(GameConfigScriptable gameConfig)
        {
            IConfiguration bindsConfiguration = new DefaultBinds();
            SetConfig<IContainerConfig>(new DefaultContainerConfig());
            SetConfig<IBindsConfiguration>(bindsConfiguration);
            
            return await UniTask.FromResult(Instance);
        }

        /// <summary>
        /// Add config to manager cache
        /// </summary>
        public void SetConfig<T>(IConfiguration config) where T : class, IConfiguration
        {
            if (config == null) throw new ArgumentNullException($"Config is null!");
            _cache.TryAdd(typeof(T), config);
        }

        public T GetConfiguration<T>() where T : class, IConfiguration
        {
            if (!_cache.ContainsKey(typeof(T)))
                throw new KeyNotFoundException($"{typeof(T)} / Does not exist in configs cache");
            return _cache[typeof(T)] as T;
        }

        public async UniTask Load(Action<ILoadable> action)
        {
            action.Invoke(this);
            await UniTask.CompletedTask;
        }
    }
}