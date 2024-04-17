using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.Core;
using Sources.Scripts.DI;

namespace Sources.Scripts.TestConfig
{
    /// <summary>
    /// Bind services 
    /// </summary>
    public interface IBindsConfig
    {
        public async Task InitBindings(Container container)
        {
            await Task.CompletedTask;
        }
    }
}