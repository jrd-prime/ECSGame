using System;
using System.Collections.Generic;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Core.Config;
using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.TestDB;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.DI
{
    /// <summary>
    /// Init services and inject dependencies
    /// </summary>
    public sealed class AppContext : MonoBehaviour
    {
        [SerializeField] private IBindsConfiguration _bindsConfiguration;
        [SerializeField] private GameConfigScriptable _gameConfig;

        private MyContainer _myContainer;

        public static AppContext I { get; private set; }

        // Config
        private IConfiguration _currentConfig;
        private IServiceFactory _serviceFactory;
        private IDataBase _dataBase;

        private void Awake()
        {
            I = this;
            DontDestroyOnLoad(this);
        }

        public async Task InitializeAsync()
        {
            // 1. Init Container. Required for: bind, cache, auto-inject
            {
                // 1. Instance
                _myContainer = new MyContainer();
                _serviceFactory = ServiceFactoryManager.I.GetFactory(_gameConfig._serviceFactory);
                _currentConfig = ConfigurationManager.I.GetConfiguration(_bindsConfiguration);

                // 2. Inject new() instances in Container
                Dictionary<Type, object> instances = await ReflectionUtils.ManualInjectAsync(_myContainer);

                // 3. Inject factory from config in Container Cache
                // NOTE: get Container Cache instance and then inject IServiceFactory into it
                await ReflectionUtils.ManualInjectWithInstanceAsync(instances[typeof(ContainerCache)], _serviceFactory);

                // 4. Bind Container
                await _myContainer.BindAsync(_myContainer);
                JLog.Msg("\t(!) Container initialization complete!");
            }

            // 2. Bind(+cache) all services from configs
            await _myContainer.BindConfigAsync(_currentConfig);
            JLog.Msg("\t(!) Bind services from config complete!");

            // 3. Inject services from cache to assembly by attr: [JInject]
            await _myContainer.InjectServicesAsync(Assembly.GetExecutingAssembly());
            JLog.Msg("\t(!) Dependency injection complete!");
        }

        private void OnValidate()
        {
            if (_gameConfig == null)
                throw new Exception($"Game config not set to {gameObject.name}!");
        }
    }
}