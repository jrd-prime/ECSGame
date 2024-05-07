using System;
using System.Collections.Generic;
using ECSGame.Scripts.Core.Config.Interface;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface IContainerConfig : IConfiguration
    {
        public Dictionary<Type, Type> GetConfig();
    }
}