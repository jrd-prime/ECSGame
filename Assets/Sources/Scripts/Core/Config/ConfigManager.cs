using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Sources.Scripts.Core.Config
{
    public class ConfigManager
    {
        private static ConfigManager _configManager;
        public static ConfigManager I => _configManager ??= new ConfigManager();

        private static Dictionary<Type, object> _cache;

        private ConfigManager()
        {
            _cache = new Dictionary<Type, object>();
        }

        public void SetConfig<T>(ref IConfiguration config) where T :class, IConfiguration
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
    }
}