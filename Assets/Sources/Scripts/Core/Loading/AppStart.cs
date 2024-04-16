

using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.Core.Loading
{
    public sealed class AppStart : MonoBehaviour
    {
        [SerializeField] private GameObject _contextHolder;

        private void Start()
        {
            JLog.Msg("App Start");


            // Services config
            IServiceConfig serviceConfig = new NetServiceConfig();

            #region PreInit

            AppContext context = _contextHolder.GetComponent<AppContext>();
            Container container = new();
            ServiceFactory serviceFactory = new();

            // Mini app start hand inject
            ReflectionUtils.HandInject(context, serviceConfig);
            ReflectionUtils.HandInject(context, container);
            ReflectionUtils.HandInject(container, serviceFactory);

            #endregion

             context.Initialize();

            // context.SetServiceFactory(serviceFactory);
            // context.SetContainer(container);


            // context.BindServicesFromConfig(serviceConfig);

            // container.AddToCache(typeof(DBController), gameObject.AddComponent<DBController>());

            // context.Inject(Assembly.GetExecutingAssembly());
        }
    }
}