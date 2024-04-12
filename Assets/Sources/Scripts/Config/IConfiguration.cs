using System;
using System.Collections.Generic;

namespace Sources.Scripts.ServiceConfig
{
    public interface IConfiguration
    {
        public string Name { get; }
        public Dictionary<Type, object> GetImpl();
    }
}