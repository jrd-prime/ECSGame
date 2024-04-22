using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.Core;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DI;

namespace Sources.Scripts.TestConfig
{
    /// <summary>
    /// Bind services 
    /// </summary>
    public interface IBindsConfiguration: IBindableConfiguration
    {
        public async Task InitBindings(MyContainer myContainer)
        {
            await Task.CompletedTask;
        }
    }
}