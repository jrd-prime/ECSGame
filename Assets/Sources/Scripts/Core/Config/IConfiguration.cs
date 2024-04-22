using System;
using System.Collections.Generic;

namespace Sources.Scripts.Core.Config
{
    public interface IConfiguration
    {
        public Dictionary<Type, Type> GetBindings();
    }
}