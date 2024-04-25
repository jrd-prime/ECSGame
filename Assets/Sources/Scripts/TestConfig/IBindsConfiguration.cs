using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Sources.Scripts.Core;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DI;
using Sources.Scripts.DI.Interface;

namespace Sources.Scripts.TestConfig
{
    /// <summary>
    /// Bind services 
    /// </summary>
    public interface IBindsConfiguration: IBindableConfiguration
    {
        public async Task InitBindings(IMyContainer container)
        {
            await Task.CompletedTask;
        }
    }
}