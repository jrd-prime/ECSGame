using System;
using ECSGame.Scripts.Core.Config.Enum;

namespace ECSGame.Scripts.Core.DI.Factory
{
    public class ServiceFactoryManager
    {
        private static ServiceFactoryManager _serviceFactoryManager;
        public ServiceFactoryEnum currentFactoryEnum { get; private set; }
        public static ServiceFactoryManager I => _serviceFactoryManager ??= new ServiceFactoryManager();

        private ServiceFactoryManager()
        {
        }

        public Type GetFactoryType(ServiceFactoryEnum serviceFactory)
        {
            currentFactoryEnum = serviceFactory;

            return serviceFactory switch
            {
                ServiceFactoryEnum.Standard =>  typeof(DefaultServiceFactory),
                _ => throw new ArgumentOutOfRangeException(nameof(serviceFactory), serviceFactory, null)
            };
        }

        public Type GetCurrentFactory()
        {
            return GetFactoryType(currentFactoryEnum);
        }
    }
}