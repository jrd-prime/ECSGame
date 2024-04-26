using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
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
    public class AppContext : MonoBehaviour, IAppContext
    {
        [SerializeField] public GameConfigScriptable _gameConfig;

        private IContainerProvider _containerProvider;
        private IMyContainer _container;

        public async Task InitializeAsync()
        {
            JLog.Msg("\t(!) Initialization started!");

            await InitAndSetConfigs();
            await InitAndBindContainer();
            await BindServicesFromConfigs();
            await InjectServicesFromCacheToAssembly();
        }


        private async UniTask InitAndSetConfigs()
        {
            var a = new TaskCompletionSource<bool>();
            Type serviceFactoryType = ServiceFactoryManager.I.GetFactoryType(_gameConfig._serviceFactory);
            IConfiguration bindsConfiguration = new TestBinds();
            IConfiguration containerConfig = new DefaultContainerConfig
            {
                Impl = new Dictionary<Type, Type>
                {
                    { typeof(IMyContainer), typeof(MyContainer) },
                    { typeof(IBinder), typeof(Binder) },
                    { typeof(ICache), typeof(Cache) },
                    { typeof(IInjector), typeof(Injector) },
                    { typeof(IServiceFactory), serviceFactoryType }
                }
            };

            ConfigManager.I.SetConfig<IContainerConfig>(ref containerConfig);
            ConfigManager.I.SetConfig<IBindsConfiguration>(ref bindsConfiguration);

            a.TrySetResult(true);

            await a.Task;
        }

        private async UniTask InitAndBindContainer()
        {
            // 1. Init & bind Container.
            _containerProvider = new DefaultContainerProvider();
            _container = await _containerProvider.GetContainer();
            await _container.BindAsync<IMyContainer>(_container);
            JLog.Msg("\t(!) Container initialization complete!");
        }

        private async UniTask BindServicesFromConfigs()
        {
            // 2. Bind(+cache) all services from configs
            var bindsConfig = ConfigManager.I.GetConfiguration<IBindsConfiguration>();
            await _container.BindConfigAsync(bindsConfig);
            JLog.Msg("\t(!) Bind services from config complete!");
        }

        private async UniTask InjectServicesFromCacheToAssembly()
        {
            // 3. Inject services from cache to assembly by attr: [JInject]
            await _container.InjectServicesAsync(Assembly.GetExecutingAssembly());
            JLog.Msg("\t(!) Dependency injection complete!");
        }

#if !UNITY_INCLUDE_TESTS
        private void OnValidate()
        {
            if (_gameConfig == null) throw new Exception($"Game config not set to {gameObject.name}!");
        }
#endif
    }
}