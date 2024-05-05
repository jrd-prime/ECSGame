using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.DI.Factory;

namespace ECSGame.Scripts.Core.Config.Providers
{
    public class ServiceFactoryProvider : CustomProvider
    {
        protected override Dictionary<Enum, Type> GetDictWithImpl()
        {
            return new Dictionary<Enum, Type>
            {
                { ServiceFactorySelect.Standard, typeof(DefaultServiceFactory) }
            };
        }
    }

    public enum ServiceFactorySelect
    {
        Standard,
        Cloud
    }
}