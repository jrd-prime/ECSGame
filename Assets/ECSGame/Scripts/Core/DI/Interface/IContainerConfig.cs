using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.Config;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface IContainerConfig : IConfiguration
    {
        public Dictionary<Type, Type> Impl { get; set; }
    }
}