using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.DataBase;
using UnityEngine;

namespace ECSGame.Scripts.Core.Config.Providers
{
    public class Providers
    {
        private static bool _isInit;
        private static Providers _instance;

        public static readonly List<Type> ProvidersList = new()
        {
            typeof(ContainerProvider),
            typeof(ContainerFactoryProvider),
            typeof(ServiceFactoryProvider),
            typeof(DataBaseProvider),
            typeof(AssetLoaderProvider)
        };

        private static readonly Dictionary<Type, CustomProvider> ProvidersCache = new();

        private Providers()
        {
        }

        public static Providers Init()
        {
            foreach (var provider in ProvidersList)
            {
                ProvidersCache.Add(provider, Activator.CreateInstance(provider) as CustomProvider);

                Debug.LogWarning($"added = {ProvidersCache[provider]}");
            }

            _isInit = true;
            return _instance ??= new Providers();
        }

        public CustomProvider Get<T>()
        {
            if (!_isInit) throw new Exception("Providers no init!");

            if (!ProvidersCache.TryGetValue(typeof(T), out CustomProvider provider))
                throw new KeyNotFoundException($"{typeof(T)} not in cache! Add first");
            return provider;
        }
    }
}