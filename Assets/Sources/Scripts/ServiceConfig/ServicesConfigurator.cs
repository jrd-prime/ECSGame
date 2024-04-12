using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Sources.Scripts.ServiceConfig
{
    public class ServicesConfigurator : IServiceConfigurator
    {
        private readonly Dictionary<Type, object> _impl = new();

        public T GetImpl<T>() where T : class
        {
            T typeInstance = null;


            var subTypes = GetSubTypesOf(typeof(T)).ToList();

            if (_impl.ContainsKey(typeof(T)))
            {
                Debug.LogWarning("--- impl in dict");
                var a = _impl[typeof(T)];
                // return (T)Activator.CreateInstance(a);

                return (T)a;
            }
            else
            {
                Debug.LogWarning("--- NO impl in dict");
                Debug.Log("type start");
                foreach (var variablType in subTypes)
                {
                    Debug.Log(variablType.FullName);
                }

                Debug.Log("type end");
                typeInstance = (T)Activator.CreateInstance(subTypes.First());

                _impl.Add(typeof(T), typeInstance);

                return typeInstance;
            }
        }

        private IEnumerable<Type> GetSubTypesOf(Type type)
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