using System;
using Sources.Scripts.Core.Config.Enum;

namespace Sources.Scripts.Factory
{
    public class ServiceFactoryManager
    {
        private static ServiceFactoryManager _serviceFactoryManager;
        public ServiceFactoryEnum CurrentFactory { get; private set; }
        public static ServiceFactoryManager I => _serviceFactoryManager ??= new ServiceFactoryManager();

        private ServiceFactoryManager()
        {
        }

        public IServiceFactory GetFactory(ServiceFactoryEnum serviceFactory)
        {
            CurrentFactory = serviceFactory;

            return serviceFactory switch
            {
                ServiceFactoryEnum.Standard => new StandardServiceFactory(),
                ServiceFactoryEnum.Cloud => new CloudServiceFactory(),
                _ => throw new ArgumentOutOfRangeException(nameof(serviceFactory), serviceFactory, null)
            };
        }
    }
}