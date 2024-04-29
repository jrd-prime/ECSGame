using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ECSGame.Scripts.Core.DI.Interface;

namespace ECSGame.Scripts.TestConfig
{
    public class TestBinds : IBindsConfiguration
    {
        public async Task InitBindings(IMyContainer myContainerFull)
        {
            await Task.CompletedTask;
        }

        public Dictionary<Type, Type> GetBindings()
        {
            return new Dictionary<Type, Type>()
            {
                { typeof(IServiceConfig), typeof(NetServiceConfig) }
            };
        }
    }
}