using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Annotation;
using Sources.Scripts.Factory;
using Sources.Scripts.TestConfig;
using Sources.Scripts.Utils;
using UnityEngine;

namespace Sources.Scripts.DI
{
    public class Container
    {
        [JHandInject] private ServiceFactory _serviceFactory;

        private readonly Dictionary<Type, Type> _binds = new();
        private readonly Dictionary<Type, object> _instancesCache = new();

        public Dictionary<Type, object> GetCache() => _instancesCache;

        public async Task InitServicesAsync(Dictionary<Type, Type> services)
        {
            JLog.Msg($"({services.Count}) Services initialization STARTED...");

            foreach (var service in services)
            {
                await Bind(service.Key, service.Value);
            }

            JLog.Msg($"({services.Count}) Services initialization FINISHED...");
        }

        public async Task Bind(Type baseType, Type implementationType)
        {
            _binds.TryAdd(baseType, implementationType);
            await CreateAndAddToCache(baseType);
        }

        public async Task Bind<TBase, TImplementation>()
            where TBase : class
            where TImplementation : class
        {
            _binds.TryAdd(typeof(TBase), typeof(TImplementation));
            await CreateAndAddToCache(typeof(TBase));
        }

        public T Get<T>() where T : class, new()
        {
            return new T();
        }


        private async Task CreateAndAddToCache(Type baseType)
        {
            if (!_instancesCache.ContainsKey(baseType))
            {
                var implType = _binds[baseType];
                var instance = _serviceFactory.GetService(implType);

                _instancesCache.Add(baseType, instance);

                JLog.Msg(
                    $"Not in cache. Create and add: {Helper.TypeNameCutter(baseType)} > {Helper.TypeNameCutter(implType)}");
            }

            await Task.Delay(2000);
        }


        public async Task AddToCache(Type target, object value)
        {
            _instancesCache.TryAdd(target, value);
            await Task.CompletedTask;
        }

        public void Description()
        {
            JLog.Msg("Container description");
        }

        public async Task InjectDependenciesAsync(Assembly assembly)
        {
            JLog.Msg($"( InjectDependenciesAsync STARTED...");

            foreach (var type in assembly.GetTypes())
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (var field in fields)
                {
                    if (!Attribute.IsDefined(field, typeof(JInject))) continue;


                    var value = GetCache()[field.FieldType];
                    Debug.LogWarning("value =  " + value);
                    Debug.LogWarning("fi type =  " + field.FieldType);

                    bool isTypeEqual = field.FieldType == value.GetType();
                    bool isImplementInterface = value.GetType().GetInterfaces().Contains(field.FieldType);

                    // if (!isTypeEqual && !isImplementInterface) continue;

                    if (!isTypeEqual) continue;

                    Debug.LogWarning($"TYPE WITH INJECT = {type}");

                    Debug.LogWarning($"Inject here: {type} -> {field.FieldType} -> {value.GetType()}");


                    field.SetValue(Activator.CreateInstance(field.FieldType), value);

                    // for logging
                    var val = value.GetType().ToString().Split('.').Last();
                    var tar = value.ToString().Split('.').Last();
                    JLog.Msg($"Injected. {val} to {tar}");
                }
            }

            JLog.Msg($"( InjectDependenciesAsync FINISHED...");

            // object target;
            // foreach (var type in assembly.GetTypes())
            // {
            //     var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
            //
            //     foreach (var fieldInfo in fields)
            //     {
            //         if (!Attribute.IsDefined(fieldInfo, typeof(JInject))) continue;
            //
            //         JLog.Msg("INJECT HERE = " + type);
            //
            //
            //         var injectType = fieldInfo.FieldType;
            //         GetCache().TryGetValue(injectType, out var injectValue);
            //
            //         JLog.Builder().AddLine(injectType).AddLine(injectValue).Build();
            //
            //
            //         if (GetCache().TryGetValue(injectType, out var value))
            //         {
            //             Debug.Log("Return from cache");
            //             injectValue = value;
            //         }
            //
            //         target = GetCache()[type];
            //         Debug.Log("Not in cache");
            //
            //         Debug.LogWarning("======");
            //         Debug.Log(fieldInfo);
            //         // Debug.Log(target);
            //         Debug.LogWarning(injectType);
            //         Debug.LogWarning(injectValue);
            //         Debug.LogWarning("======");
            //         fieldInfo.SetValue(target, injectValue);
            //
            //         Debug.LogWarning($"(!) Injected: {fieldInfo.FieldType} to {type.Name}");
            //     }
            // }

            await Task.CompletedTask;
        }

        public async Task InitBinds(IBindsConfig bindsConfig)
        {
        }

        public void Bind(Type type)
        {
            _binds.TryAdd(type, type);
        }

        public async Task Bind(IEnumerable dict)
        {
            await Task.CompletedTask;
        }

        public async Task<T> BindSelfAsync<T>() where T : class
        {
            if (_binds.TryAdd(typeof(T), typeof(T)))
            {
                JLog.Msg($"BindSelf -> {typeof(T)}");
            }

            T instance = Activator.CreateInstance<T>();

            await AddToCache(typeof(T), instance);

            return instance;
        }

        public async Task<T> BindSelfAsync<T>(GameObject gameObject) where T : class
        {
            if (_binds.TryAdd(typeof(T), typeof(T)))
            {
                JLog.Msg($"BindSelf -> {typeof(T)}");
            }

            T instance = gameObject.GetComponent<T>();
            await AddToCache(typeof(T), instance);

            return instance;
        }

        public void BindConfig()
        {
            // TODO
        }

        public Dictionary<Type, Type> getBinds()
        {
            return _binds;
        }

        public async Task BindInterface<TInterface, TImplementation>()
            where TInterface : class
            where TImplementation : class
        {
            if (_binds.TryAdd(typeof(TInterface), typeof(TImplementation)))
            {
                JLog.Msg($"BindInterface -> {typeof(TInterface)} -> {typeof(TImplementation)}");
            }

            await AddToCache(typeof(TInterface), Activator.CreateInstance<TImplementation>());
        }
    }
}


// private void Bind(Type type, object imp)
// {
//     if (_instancesCache.TryAdd(type, imp)) Debug.Log("Added to cache!");
// }

// public T InjectDependencies<T>() where T : class, new()
// {
//     Debug.Log("INJECT THIS = " + typeof(T));
//     var assembly = Assembly.GetAssembly(typeof(T));
//
//     Debug.LogWarning(assembly);
//
//     T target = new T();
//
//     foreach (var type in assembly.GetTypes())
//     {
//         var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);
//
//         foreach (var fieldInfo in fields)
//         {
//             if (!Attribute.IsDefined(fieldInfo, typeof(JInject))) continue;
//
//             var injectType = fieldInfo.FieldType;
//             // var injectValue = _serviceFactory.GetService(injectType);
//
//             Debug.LogWarning(injectType);
//             // Debug.LogWarning(injectValue);
//
//
//             if (_servicesCache.TryGetValue(injectType, out var value))
//             {
//                 Debug.Log("Return from cache");
//                 // injectValue = value;
//             }
//
//             Debug.Log("Not in cache");
//
//             // Bind(injectType, injectValue);
//             Debug.LogWarning("======");
//             Debug.Log(fieldInfo);
//             Debug.Log(target);
//             Debug.LogWarning(injectType);
//             // Debug.LogWarning(injectValue);
//             Debug.LogWarning("======");
//             // fieldInfo.SetValue(target, injectValue);
//
//             Debug.LogWarning($"(!) Injected: {fieldInfo.FieldType} to {type.Name}");
//         }
//     }
//
//     return target;
// }