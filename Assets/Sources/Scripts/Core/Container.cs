using System;
using System.Collections.Generic;
using System.Reflection;
using Sources.Scripts.Annotation;
using Sources.Scripts.Factory;
using UnityEngine;

namespace Sources.Scripts.Core
{
    public sealed class Container
    {
        private readonly ServiceFactory _serviceFactory;
        private readonly Dictionary<Type, object> _servicesCache;

        public Dictionary<Type, object> GetCache() => _servicesCache;

        public Container(ServiceFactory serviceFactory)
        {
            _servicesCache = new Dictionary<Type, object>();
            _serviceFactory = serviceFactory;
        }

        private void Bind(Type type, object imp)
        {
            if (_servicesCache.TryAdd(type, imp)) Debug.Log("Added to cache!");
        }

        public T InjectDependencies<T>() where T : class, new()
        {
            Debug.Log("INJECT THIS = " + typeof(T));
            var assembly = Assembly.GetAssembly(typeof(T));

            Debug.LogWarning(assembly);

            T target = new T();

            foreach (var type in assembly.GetTypes())
            {
                var fields = type.GetFields(BindingFlags.Instance | BindingFlags.NonPublic);

                foreach (var fieldInfo in fields)
                {
                    if (!Attribute.IsDefined(fieldInfo, typeof(JInject))) continue;

                    var injectType = fieldInfo.FieldType;
                    var injectValue = _serviceFactory.GetService(injectType);

                    Debug.LogWarning(injectType);
                    Debug.LogWarning(injectValue);


                    if (_servicesCache.TryGetValue(injectType, out var value))
                    {
                        Debug.Log("Return from cache");
                        injectValue = value;
                    }

                    Debug.Log("Not in cache");

                    Bind(injectType, injectValue);
                    Debug.LogWarning("======");
                    Debug.Log(fieldInfo);
                    Debug.Log(target);
                    Debug.LogWarning(injectType);
                    Debug.LogWarning(injectValue);
                    Debug.LogWarning("======");
                    fieldInfo.SetValue(target, injectValue);

                    Debug.LogWarning($"(!) Injected: {fieldInfo.FieldType} to {type.Name}");
                }
            }

            return target;
        }

        public void AddToCache(Type serviceKey, object o)
        {
            if (_servicesCache.TryAdd(serviceKey, o))
            {
                Debug.Log($"Container. Service added to cache {serviceKey} / {o}");
            }
        }
    }
}