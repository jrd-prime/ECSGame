using System;
using System.Collections.Generic;
using Sources.Scripts.DI.Interface;

namespace Sources.Scripts.DI
{
    public struct DefaultContainerConfig : IContainerConfig
    {
        public Dictionary<Type, Type> Impl { get; set; }

    }
}