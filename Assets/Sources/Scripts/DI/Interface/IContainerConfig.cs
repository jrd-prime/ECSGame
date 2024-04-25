using System;
using System.Collections.Generic;
using Sources.Scripts.Core.Config;

namespace Sources.Scripts.DI.Interface
{
    public interface IContainerConfig : IConfiguration
    {
        public Dictionary<Type, Type> Impl { get; set; }
    }
}