using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Scripts.ServiceConfig
{
    public class ServicesConfigurator : IServiceConfigurator
    {
        private readonly Dictionary<Type, object> _implCache;

        public ServicesConfigurator(IDictionary<Type, object> implFromConfig)
        {
            _implCache = new Dictionary<Type, object>(implFromConfig);
        }

        public T GetImpl<T>() where T : class
        {
            if (_implCache.ContainsKey(typeof(T)))
            {
                Debug.LogWarning("--- impl in dict");
                return (T)_implCache[typeof(T)];
            }

            Debug.LogWarning("--- NO impl in dict");

            var subTypes = GetSubTypesOf(typeof(T)).ToList();

            var typeInstance = (T)Activator.CreateInstance(subTypes.First());

            _implCache.Add(typeof(T), typeInstance);

            return typeInstance;
        }

        private static IEnumerable<Type> GetSubTypesOf(Type type)
        {
            var assemblies = AppDomain.CurrentDomain.GetAssemblies();

            List<Type> types = new();

            foreach (var assembly in assemblies)
            {
                foreach (var subtype in assembly.GetTypes())
                {
                    if (!subtype.GetInterfaces().Contains(type))
                        continue;

                    types.Add(subtype);
                }
            }

            return types;
        }
    }
}