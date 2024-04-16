using System;
using Sources.Scripts.Annotation;
using UnityEngine;
using AppContext = Sources.Scripts.Core.AppContext;

namespace Sources.Scripts.Factory
{
    /// <summary>
    /// Get service realization by <see cref="IConfiguration"/> config
    /// </summary>
    public class ServiceFactory
    {
        // private readonly IConfiguration _configuration;
        // private readonly IServiceConfigurator _serviceConfigurator;

        // public ServiceFactory(AppContext context)
        // {
        //     _context = context;
        //     // _serviceConfigurator = new ServicesConfigurator(_configuration.GetImpl());
        // }

        public T GetService<T>() where T : class
        {
            Debug.Log("In get service");
        
            T service = null;
            // if (!typeof(T).IsInterface)
            // {
            //     service = Activator.CreateInstance<T>();
            // }
            // else
            // {
            //     service = _serviceConfigurator.GetImpl<T>();
            // }
            
            var serviceFields = service.GetType().GetFields();
            
            foreach (var fieldInfo in serviceFields)
            {
                var hasAttr = Attribute.IsDefined(fieldInfo, typeof(JInject));
            
                // fieldInfo.SetValue(service, _context.GetService<T>());
                Debug.Log(hasAttr + " ===");
            }
            
            return service;
        }

        public object GetService(Type serviceValue)
        {
            return Activator.CreateInstance(serviceValue);
        }
    }
}