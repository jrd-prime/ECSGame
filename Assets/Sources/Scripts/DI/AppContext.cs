using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DI.Interface;
using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.DI
{
    /// <summary>
    /// Init services and inject dependencies
    /// </summary>
    public sealed class AppContext : MonoBehaviour
    {
        [SerializeField] private GameConfigScriptable _gameConfig;
        private IContainerProvider _containerProvider;

        public static AppContext I { get; private set; }

        private void Awake()
        {
            I = this;
            DontDestroyOnLoad(this);
        }

        public async Task InitializeAsync()
        {
            // 0. Init & set configs
            Type serviceFactoryType = ServiceFactoryManager.I.GetFactoryType(_gameConfig._serviceFactory);
            IConfiguration bindsConfiguration = new TestBinds();
            IConfiguration containerConfig = new DefaultContainerConfig
            {
                Impl = new Dictionary<Type, Type>
                {
                    { typeof(IMyContainer), typeof(MyContainer) },
                    { typeof(IContainerBinder), typeof(ContainerBinder) },
                    { typeof(IContainerCache), typeof(ContainerCache) },
                    { typeof(IContainerInjector), typeof(ContainerInjector) },
                    { typeof(IServiceFactory), serviceFactoryType }
                }
            };

            ConfigurationManager.I.SetConfig<IContainerConfig>(ref containerConfig);
            ConfigurationManager.I.SetConfig<IBindsConfiguration>(ref bindsConfiguration);


            // 1. Init & bind Container.
            _containerProvider = new DefaultContainerProvider();
            var container = _containerProvider.GetContainer();
            await container.BindAsync<IMyContainer>(container);
            JLog.Msg("\t(!) Container initialization complete!");


            // 2. Bind(+cache) all services from configs
            var bindsConfig = ConfigurationManager.I.GetConfiguration<IBindsConfiguration>();
            await container.BindConfigAsync(bindsConfig);
            JLog.Msg("\t(!) Bind services from config complete!");

            // 3. Inject services from cache to assembly by attr: [JInject]
            await container.InjectServicesAsync(Assembly.GetExecutingAssembly());
            JLog.Msg("\t(!) Dependency injection complete!");
        }

        private void OnValidate()
        {
            if (_gameConfig == null) throw new Exception($"Game config not set to {gameObject.name}!");
        }
    }
}