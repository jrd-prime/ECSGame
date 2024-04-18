using Sources.Scripts.DI;
using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.Utils;
using UnityEngine;
using AppContext = Sources.Scripts.DI.AppContext;

namespace Sources.Scripts.Core.Loading
{
    public sealed class AppStart : MonoBehaviour
    {
        [SerializeField] private GameObject _appContextHolderGo;
        private AppContext _context;
        private Container _container;
        private ServiceFactory _serviceFactory;
        private IBindsConfig _bindsConfig;

        private void Awake()
        {
            DontDestroyOnLoad(_appContextHolderGo);
            _context = _appContextHolderGo.GetComponent<AppContext>();
            _container = new Container();
            _bindsConfig = new TestBinds();
        }

        private async void Start()
        {
            // ImportantBindings
            await _container.Bind<Container>();
            await _container.Bind<AppContext>(_context);
            _serviceFactory = await _container.Bind<ServiceFactory>();
            
            // Bindings
            await _bindsConfig.InitBindings(_container);

            // HandInjection by attr: [JHandInject]
            var reflectionUtils = new ReflectionUtils(_container);
            reflectionUtils.HandInject(_context, _bindsConfig);
            reflectionUtils.HandInject(_context, _container);
            reflectionUtils.HandInject(_container.Cache, _serviceFactory);

            // AutoInjection by attr: [JInject]
            await _context.InitializeAsync();


// TODO on app start and fake loading services - create loading screen

// TODO add asset loadin async init binds

            Debug.LogWarning($"in cache binds: {_container.Binder.GetBinds().Count}".ToUpper());
            Debug.LogWarning($"in cache instances: {_container.Cache.GetCache().Count}".ToUpper());

            Debug.LogWarning("INITIALISATION FINISHEEEDDDD!!!!");
            Debug.LogWarning("STAAAART THHHE GAMEE");
        }
    }
}