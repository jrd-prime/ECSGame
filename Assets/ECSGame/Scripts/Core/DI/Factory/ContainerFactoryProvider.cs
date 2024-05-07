using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.Config.Providers;

namespace ECSGame.Scripts.Core.DI.Factory
{
    public class ContainerFactoryProvider : CustomProvider
    {
        protected override Dictionary<Enum, Type> GetDictWithImpl()
        {
            return new Dictionary<Enum, Type>
            {
                { ContainerFactorySelect.Default, typeof(DefaultContainerFactory) }
            };
        }
    }

    public enum ContainerFactorySelect
    {
        Default
    }
}