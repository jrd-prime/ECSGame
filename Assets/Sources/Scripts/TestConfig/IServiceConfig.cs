using System;
using System.Collections.Generic;

namespace Sources.Scripts.TestConfig
{
    public interface IServiceConfig
    {
        public Dictionary<Type, Type> GetServicesList();
    }
}