using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.Annotation;
using ECSGame.Scripts.Core.Config;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.Utils;

namespace ECSGame.Scripts.Core.DI
{
    public class Cache : ICache
    {
        [JManualInject] private readonly IServiceFactory _serviceFactory;

        private static readonly Dictionary<Type, object> CacheDict = new();

        public bool IsFieldsInjected() => _serviceFactory != null;
        public Dictionary<Type, object> GetCache() => CacheDict;

        public void Add(in IBindableConfiguration tempConfig)
        {
            if (tempConfig == null) throw new ArgumentNullException();

            var binds = tempConfig.GetBindings();

            if (binds.Count == 0) return;

            foreach (var bind in binds)
            {
                AddMain(bind.Key, bind.Value);
            }
        }

        public void Add(Type baseType, Type implType, in object implInstance = null)
        {
            if (baseType == null || implType == null) throw new ArgumentNullException();
            if (CacheDict.ContainsKey(baseType)) return;

            object instance = implInstance ?? _serviceFactory.CreateService(implType);

            AddMain(baseType, in instance);
        }

        private void AddMain(Type type, in object instance)
        {
            if (type == null || instance == null) throw new ArgumentNullException();

            if (CacheDict.TryAdd(type, instance)) JLog.Msg($"Added to cache -> {type} <- {instance}");
        }

        //TODO BindAndGet ? bad idea?
        public T Get<T>() where T : class
        {
            if (!CacheDict.ContainsKey(typeof(T)))
                throw new KeyNotFoundException(
                    $"{typeof(T)} >>> The service is not in cache! First, bind the service!");

            return CacheDict[typeof(T)] as T;
        }

        public void Clear()
        {
            CacheDict.Clear();
            JLog.Msg("(!!!) Cache cleared!".ToUpper());
        }
    }
}