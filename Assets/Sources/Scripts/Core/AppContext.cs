using System;
using System.Reflection;
using System.Threading.Tasks;
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
        
        private AppContext _context;

        public Container Container { get; private set; }

        private void Awake()
        {
            DontDestroyOnLoad(this);
        }

        private void Start()
        {
            JLog.Msg("Simple one line msg");
            _container.Description();
        }


        public void BindServicesFromConfig(IServiceConfig serviceConfig)
        {
        }

        public T GetService<T>() where T : class
        {
            var cache = Container.GetCache();

            if (cache.ContainsKey(typeof(T)))
                return (T)cache[typeof(T)];

            T service = Container.getSer().GetService<T>();
            Container.AddToCache(typeof(T), service);

            return service;
        }
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

                    JLog.Msg("INJECT HERE = " + type);

                    target = Container.GetCache()[type];

                    var injectType = fieldInfo.FieldType;
                    Container.GetCache().TryGetValue(injectType, out var injectValue);

                    JLog.Builder().AddLine(injectType).AddLine(injectValue).Build();


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

        public async void Initialize()
        {
            JLog.Msg("Start init services");
            await InitializeServices();
            JLog.Msg("services loaded");

            JLog.Msg("start inject");
            Inject(Assembly.GetExecutingAssembly());
        }

        private async Task InitializeServices()
        {
            var services = _serviceConfig.Init();

            JLog.Msg(services.Count);

            foreach (var service in services)
            {
                JLog.Msg(service.Key + " + " + service.Value);
                var a = Container.getSer().GetService(service.Value);
                Container.AddToCache(service.Key, a);
                await Task.Delay(1000);
                JLog.Msg($"{service} Loaded");
            }

            await Task.CompletedTask;
        }
    }
}