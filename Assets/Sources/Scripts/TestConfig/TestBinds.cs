using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.DI;

namespace Sources.Scripts.TestConfig
{
    public class TestBinds : IBindsConfig
    {
        public async Task InitBindings(Container container)
        {
            await Task.CompletedTask;
        }

        public Dictionary<Type, Type> GetBindingsList()
        {
            return new Dictionary<Type, Type>()
            {
                { typeof(IServiceConfig), typeof(NetServiceConfig) }
            };
        }
    }
}