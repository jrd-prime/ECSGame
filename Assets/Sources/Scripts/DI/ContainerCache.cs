using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.Factory;
using Sources.Scripts.Utils;
using UnityEngine;

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


        private void AddMain(Type baseType, Type implType)
        {
            if (Cache.ContainsKey(baseType)) return;

            var instance = _serviceFactory.CreateServiceAsync(implType).Result;

            AddToCache(baseType, instance);

            JLog.Msg(
                $"Not in cache. Create and add: {Helper.TypeNameCutter(baseType)} > {Helper.TypeNameCutter(implType)}");
        }

        private async Task<T> GetFromCache<T>() where T : class
        {
            if (Cache.ContainsKey(typeof(T))) return await Task.FromResult(Cache[typeof(T)] as T);

            throw new KeyNotFoundException($"(!) {typeof(T)} does not exists in cache! Check bindings");
        }

        private static void AddToCache(Type type, object instance) => Cache.TryAdd(type, instance);

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


        // public async Task<object> Get(Type type) => await GetFromCache(type);
        public async Task<T> Get<T>() where T : class => await GetFromCache<T>();
    }
}