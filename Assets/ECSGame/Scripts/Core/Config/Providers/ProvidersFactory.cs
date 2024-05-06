using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.AssetLoader;
using ECSGame.Scripts.Core.DataBase;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.State.Loading;
using ECSGame.Scripts.TestDB;

namespace ECSGame.Scripts.Core.Config.Providers
{
    /// <summary>
    /// Contains and initializes a list of specific service implementations based on the configuration file.
    /// Returns instances of specific implementations.
    /// </summary>
    public class ProvidersFactory
    {
        private readonly Dictionary<Type, ProviderConfig> _configs = new();
        private readonly Dictionary<Type, CustomProvider> _cache = new();
        private readonly Dictionary<Type, object> _providedInstances = new();

        public ProvidersFactory(GameConfigScriptable config)
        {
            if (config == null) throw new NullReferenceException("Config is null!");

            BindConfigToProviders(config);
            CreateProviders();
            CreateProvidedInstances();
        }

        private void BindConfigToProviders(GameConfigScriptable config)
        {
            // Container
            BindConfigToProvider<ContainerProvider, IContainerInitializer>(config._containerInitializer);
            BindConfigToProvider<ContainerFactoryProvider, IContainerFactory>(config._containerFactory);
            BindConfigToProvider<ServiceFactoryProvider, IServiceFactory>(config._containerServiceFactory);
            // DataBase
            BindConfigToProvider<DataBaseProvider, IDataBase>(config._dataBase);
            // Asset Loader
            BindConfigToProvider<AssetLoaderProvider, IAssetLoader>(config._assetLoader);
        }

        private void CreateProviders()
        {
            foreach (var providerConfig in _configs)
            {
                _cache.Add(providerConfig.Key, Activator.CreateInstance(providerConfig.Key) as CustomProvider);
            }
        }

        private void BindConfigToProvider<TProvider, TProvidedInterface>(Enum config)
            where TProvider : CustomProvider
            where TProvidedInterface : ILoadable
        {
            _configs.TryAdd(
                typeof(TProvider),
                new ProviderConfig { InterfaceType = typeof(TProvidedInterface), ConfigEnum = config }
            );
        }

        private void CreateProvidedInstances()
        {
            foreach (var providerConfig in _configs)
            {
                var provider = _cache[providerConfig.Key];
                var instanceType = _configs[providerConfig.Key].InterfaceType;
                var configEnum = _configs[providerConfig.Key].ConfigEnum;

                provider.SetImpl(configEnum);
                var serviceInstance = provider.GetImplInstance(instanceType);
                _providedInstances.Add(instanceType, serviceInstance);
            }
        }

        private struct ProviderConfig
        {
            public Type InterfaceType;
            public Enum ConfigEnum;
        }

        public T GetProvidedInstance<T>() where T : ILoadable
        {
            if (!_providedInstances.ContainsKey(typeof(T)))
                throw new KeyNotFoundException($"{typeof(T)} not exist in provided instances.");

            return (T)_providedInstances[typeof(T)];
        }
    }
}