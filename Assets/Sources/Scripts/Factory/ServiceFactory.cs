using System;
using System.Collections.Generic;
using Sources.Scripts.Config;
using Sources.Scripts.ServiceConfig;
using UnityEngine;
using UnityEngine.UI;

namespace Sources.Scripts.Factory
{
    /// <summary>
    /// 
    /// </summary>
    public class ServiceFactory
    {
        private readonly IConfiguration _configuration;
        private readonly IServiceConfigurator _serviceConfigurator;

        private ServiceFactory()
        {
            _configuration = new GameConfig();
            _serviceConfigurator = new ServicesConfigurator();
        }

        public static ServiceFactory Instance { get; } = new();

        public T GetService<T>() where T : class
        {
            Debug.Log("In get service");
            Debug.Log(typeof(T));



            return !typeof(T).IsInterface
                ? Activator.CreateInstance<T>()
                : _serviceConfigurator.GetImpl<T>();
        }
    }
}