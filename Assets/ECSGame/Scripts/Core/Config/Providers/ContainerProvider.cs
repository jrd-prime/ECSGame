using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.DI.Initializer;

namespace ECSGame.Scripts.Core.Config.Providers
{
    public class ContainerProvider : CustomProvider
    {
        protected override Dictionary<Enum, Type> GetDictWithImpl()
        {
            return new Dictionary<Enum, Type>
            {
                { ContainerInitSelect.Default, typeof(DefaultContainerInitializer) }
            };
        }
    }

    public enum ContainerInitSelect
    {
        Default
    }
}