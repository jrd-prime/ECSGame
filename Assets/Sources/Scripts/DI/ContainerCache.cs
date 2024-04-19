using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.Factory;
using Sources.Scripts.Utils;

namespace Sources.Scripts.DI
{
    public class ContainerCache
    {
        private readonly ServiceFactory _serviceFactory;
        private static readonly Dictionary<Type, object> Cache = new();

        public Dictionary<Type, object> GetCache() => Cache;

        public ContainerCache()
        {
            _serviceFactory = new ServiceFactory();
        }
        
        #region Main

        private void AddMain(Type baseType, Type implType)
        {
            if (Cache.ContainsKey(baseType)) return;

            AddToCache(baseType, _serviceFactory.GetService(implType));

            JLog.Msg(
                $"Not in cache. Create and add: {Helper.TypeNameCutter(baseType)} > {Helper.TypeNameCutter(implType)}");
        }

        private static async Task<object> GetFromCache(Type type)
        {
            if (Cache.TryGetValue(type, out var value)) return await Task.FromResult(value);

            throw new KeyNotFoundException($"(!) {type} does not exists in cache! Check bindings");
        }

        private static void AddToCache(Type type, object instance) => Cache.TryAdd(type, instance);

        #endregion
        
        #region Add

        // Without instance
        public void Add<T>() where T : class => AddMain(typeof(T), typeof(T));

        public void Add<TBase, TImpl>() where TBase : class where TImpl : class
            => AddMain(typeof(TBase), typeof(TImpl));

        // With instance
        public void Add<T>(object instance) where T : class => AddToCache(typeof(T), instance);

        public void Add(Type type, object instance) => AddToCache(type, instance);

        public async Task Add(Dictionary<Type, Type> dictionary)
        {
            foreach (var bind in dictionary)
            {
               AddToCache(bind.Key, bind.Value);
            }

            await Task.CompletedTask;
        }

        #endregion

        #region Get

        public async Task<object> Get(Type type) => await GetFromCache(type);
        public async Task<T> Get<T>() where T : class => (T)await GetFromCache(typeof(T));

        #endregion
    }
}