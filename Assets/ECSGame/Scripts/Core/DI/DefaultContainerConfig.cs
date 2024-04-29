using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.DI.Interface;

namespace ECSGame.Scripts.Core.DI
{
    public struct DefaultContainerConfig : IContainerConfig
    {
        public Dictionary<Type, Type> Impl { get; set; }

    }
}