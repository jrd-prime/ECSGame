using System;
using System.Collections.Generic;

namespace ECSGame.Scripts.TestConfig
{
    public interface IServiceConfig
    {
        public Dictionary<Type, Type> GetServicesList();
    }
}