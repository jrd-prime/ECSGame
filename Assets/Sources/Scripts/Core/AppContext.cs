using System;
using System.Reflection;
using Sources.Scripts.Annotation;
using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.Core
{
    /// <summary>
    /// Init services and inject dependencies
    /// </summary>
    public sealed class AppContext : MonoBehaviour
    {
        [JHandInject] private Container _container;
        [JHandInject] private IServiceConfig _serviceConfig;

        private ServiceFactory _serviceFactory;
        private AppContext _context;

        public Container Container { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            JLogger.Msg("Simple one line msg");
            _container.Description();
        }


        public void BindServicesFromConfig(IServiceConfig serviceConfig)
        {
            var services = serviceConfig.Init();

            JLogger.Msg(services.Count);

            foreach (var service in services)
            {
                JLogger.Msg(service.Key + " + " + service.Value);
                // var a = _serviceFactory.GetService(service.Value);
                // Container.AddToCache(service.Key, a);
            }
        }

        public T GetService<T>() where T : class
        {
            var cache = Container.GetCache();

            if (cache.ContainsKey(typeof(T)))
                return (T)cache[typeof(T)];

            T service = _serviceFactory.GetService<T>();
            Container.AddToCache(typeof(T), service);

            return service;
        }

        public void SetServiceFactory(ServiceFactory factory) => _serviceFactory = factory;
        public void SetContainer(Container container) => Container = container;

        public void Inject(Assembly getExecutingAssembly)
        {
            var assembly = getExecutingAssembly;

            Debug.LogWarning(assembly);


            object target;
            foreach (var type in assembly.GetTypes())
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (var fieldInfo in fields)
                {
                    if (!Attribute.IsDefined(fieldInfo, typeof(JInject))) continue;

                    JLogger.Msg("INJECT HERE = " + type);

                    target = Container.GetCache()[type];

                    var injectType = fieldInfo.FieldType;
                    Container.GetCache().TryGetValue(injectType, out var injectValue);

                    JLogger.Builder().AddLine(injectType).AddLine(injectValue).Build();


                    if (Container.GetCache().TryGetValue(injectType, out var value))
                    {
                        Debug.Log("Return from cache");
                        injectValue = value;
                    }

                    Debug.Log("Not in cache");

                    Debug.LogWarning("======");
                    Debug.Log(fieldInfo);
                    // Debug.Log(target);
                    Debug.LogWarning(injectType);
                    Debug.LogWarning(injectValue);
                    Debug.LogWarning("======");
                    fieldInfo.SetValue(target, injectValue);

                    Debug.LogWarning($"(!) Injected: {fieldInfo.FieldType} to {type.Name}");
                }
            }

            // return target;
        }
    }
}