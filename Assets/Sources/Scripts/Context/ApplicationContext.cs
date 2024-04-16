using System;
using System.Collections.Generic;
using Sources.Scripts.Factory;

namespace Sources.Scripts.Context
{
    public class ApplicationContext
    {
        public ServiceFactory _serviceFactory { get; private set; }
        private readonly Dictionary<Type, object> _servicesCache;

        // public T GetService<T>() where T : class
        // {
        //     if (_servicesCache.ContainsKey(typeof(T)))
        //         return (T)_servicesCache[typeof(T)];
        //
        //     T service = _serviceFactory.GetService<T>();
        //     _servicesCache.Add(typeof(T), service);
        //
        //     return service;
        // }

        public void SetServiceFactory(ServiceFactory serviceFactory)
        {
            if (serviceFactory != null)
            {
                _serviceFactory = serviceFactory;
            }
            else
            {
                throw new ArgumentNullException($"ServiceFactory is NULL");
            }
        }
    }
}