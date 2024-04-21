using System;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DataBase;
using Sources.Scripts.DI;
using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.TestDB;
using Sources.Scripts.Utils;
using UnityEngine;
using AppContext = Sources.Scripts.DI.AppContext;

namespace Sources.Scripts.Core.Loading
{
    public sealed class AppStart : MonoBehaviour
    {
        [SerializeField] private GameObject _appContextHolderGo;
        [SerializeField] private IBindsConfig _bindsConfig;
        [SerializeField] private GameConfigScriptable _gameConfig;

        private AppContext _context;
        private Container _container;
        
        // Config
        private IServiceFactory _serviceFactory;
        private IDataBase _dataBase;
        
        private ReflectionUtils _reflectionUtils;

        private void Awake()
        {
            DontDestroyOnLoad(_appContextHolderGo);

            // Create main instances
            _context = _appContextHolderGo.GetComponent<AppContext>();
            _container = Container.I;

            _serviceFactory = ServiceFactoryManager.I.GetFactory(_gameConfig._serviceFactory);
            _dataBase = DataBaseManager.I.GetDB(_gameConfig._dataBase);

            _bindsConfig = new TestBinds();
        }

        private async void Start()
        {
            JLog.Msg($"Services initialization STARTED...");
            await _container.BindAsync(_serviceFactory);


            _reflectionUtils = new ReflectionUtils(_container);

            // ImportantBindings [Container, Context, ServiceFactory]
            await _container.BindAsync(_container);
            await _container.BindAsync(_context);
            await _container.BindAsync<IServiceFactory, StandardServiceFactory>();


            // Manual Inject
            // App Context
            await _container.Injector.ManualInject(_context, _bindsConfig);
            await _container.Injector.ManualInject(_context, _container);

            // 

            // Bindings
            await _bindsConfig.InitBindings(_container);

            // HandInjection by attr: [JHandInject]
            _reflectionUtils.HandInject(_context, _bindsConfig);
            _reflectionUtils.HandInject(_context, _container);
            _reflectionUtils.HandInject(_container.Cache, _serviceFactory);

            // AutoInjection by attr: [JInject]
            await _context.InitializeAsync();


            JLog.Msg($"(Services initialization FINISHED...");

            Debug.LogWarning("STAAAART THHHE GAMEE");


            // TODO on app start and fake loading services - create loading screen
            // TODO add asset loadin async init binds

            Debug.LogWarning(
                $"BINDS : {_container.Binder.GetBinds().Count} / CACHE : {_container.Cache.GetCache().Count}"
                    .ToUpper());
            foreach (var q in _container.Cache.GetCache())
            {
                Debug.LogWarning(
                    $"Cache:\t{Helper.TypeNameCutter(q.Key)}\t \t<- {Helper.TypeNameCutter(q.Value.GetType())}");
            }
        }

        private void OnValidate()
        {
            if (_appContextHolderGo == null)
                throw new Exception($"AppContextHolder not set to {gameObject.name}!");

            if (_gameConfig == null)
                throw new Exception($"Game config not set to {gameObject.name}!");
        }
    }
}