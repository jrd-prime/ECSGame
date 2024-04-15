using System.Reflection;
using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.TestDB;
using UnityEngine;

namespace Sources.Scripts.Core.Loading
{
    public sealed class AppStart : MonoBehaviour
    {
        private void Start()
        {
            Debug.LogWarning("App Start");
            AppContext context = AppContext.Instance;

            ServiceFactory serviceFactory = new(context);
            Container container = new(serviceFactory);

            context.SetServiceFactory(serviceFactory);
            context.SetContainer(container);

            // GAME CONFIG
            var gameConfig = new NetGameConfig();


            context.BindServicesFromConfig(gameConfig);

            container.AddToCache(typeof(DBController), gameObject.AddComponent<DBController>());

            context.Inject(Assembly.GetExecutingAssembly());
        }
    }
}