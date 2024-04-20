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
        private ReflectionUtils _reflectionUtils;

        private void Awake()
        {
            DontDestroyOnLoad(_appContextHolderGo);
            _context = _appContextHolderGo.GetComponent<AppContext>();
            _container = new Container();
            _bindsConfig = new TestBinds();
        }

        private async void Start()
        {
            JLog.Msg($"Services initialization STARTED...");
            _reflectionUtils = new ReflectionUtils(_container);
            _serviceFactory = await _container.BindAsync<ServiceFactory>() as ServiceFactory;

            // ImportantBindings
            await _container.BindAsync<Container>();
            await _container.BindAsync(_context);

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

            // foreach (var q in _container.Binder.GetBinds())
            // {
            //     Debug.LogWarning($"{q} binded");
            // }
            //
            // foreach (var q in _container.Cache.GetCache())
            // {
            //     Debug.LogWarning($"{q} cached");
            // }
            //
            // Debug.LogWarning($"in cache binds: {_container.Binder.GetBinds().Count}".ToUpper());
            // Debug.LogWarning($"in cache instances: {_container.Cache.GetCache().Count}".ToUpper());
        }
    }
}