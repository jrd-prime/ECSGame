using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.Annotation;
using Sources.Scripts.Factory;

namespace Sources.Scripts.DI
{
    public class ContainerCache
    {
       [JManualInject] private readonly IServiceFactory _serviceFactory;
        private static readonly Dictionary<Type, object> Cache = new();

        public Dictionary<Type, object> GetCache() => Cache;

        private async Task<T> GetFromCache<T>() where T : class
        {
            if (!Cache.ContainsKey(typeof(T))) throw new KeyNotFoundException();

            return await Task.FromResult(Cache[typeof(T)] as T);
        }

        private void AddMain(Type baseType, Type implType)
        {
            if (baseType == null || implType == null) throw new ArgumentNullException();
            if (Cache.ContainsKey(baseType)) return;

            var instance = _serviceFactory.CreateServiceAsync(implType).Result;

            if (instance == null) throw new NullReferenceException();

            AddToCache(baseType, in instance);
        }

        private static void AddToCache(Type type, in object instance)
        {
            if (type == null || instance == null) throw new ArgumentNullException();

            Cache.TryAdd(type, instance);
        }

        public void Add<T>() where T : class => AddMain(typeof(T), typeof(T));

        public void Add<TBase, TImpl>() where TBase : class where TImpl : class
            => AddMain(typeof(TBase), typeof(TImpl));

        public void Add<T>(in object instance) where T : class => AddToCache(typeof(T), in instance);
        public void Add(Type type, in object instance) => AddToCache(type, in instance);

        public async Task Add(Dictionary<Type, Type> dictionary)
        {
            if (dictionary == null) throw new ArgumentNullException();
            if (dictionary.Count == 0) return;

            foreach (var bind in dictionary)
            {
                AddMain(bind.Key, bind.Value);
            }

            await Task.CompletedTask;
        }

        public async Task<T> Get<T>() where T : class => await GetFromCache<T>();

        public IServiceFactory getF()
        {
            return _serviceFactory;
        }
    }
}