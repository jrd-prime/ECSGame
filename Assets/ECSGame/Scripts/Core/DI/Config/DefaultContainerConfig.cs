using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.DI.CoreParts.Binder;
using ECSGame.Scripts.Core.DI.CoreParts.Cache;
using ECSGame.Scripts.Core.DI.CoreParts.Injector;
using ECSGame.Scripts.Core.DI.Interface;
using ECSGame.Scripts.Core.DI.ServicesFactory;

namespace ECSGame.Scripts.Core.DI.Config
{
    // TODO providers with custom
    public class DefaultContainerConfig : IContainerConfig
    {
        public Dictionary<Type, Type> GetConfig()
        {
            return new Dictionary<Type, Type>
            {
                { typeof(IMyContainer), typeof(Container) },
                { typeof(IBinder), typeof(DefaultBinder) },
                { typeof(ICache), typeof(DefaultCache) },
                { typeof(IInjector), typeof(DefaultInjector) },
                {typeof(IServiceFactory), typeof(DefaultServiceFactory)}
            };
        }
    }
}