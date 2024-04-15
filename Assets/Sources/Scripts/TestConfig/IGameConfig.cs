using System;
using System.Collections.Generic;

namespace Sources.Scripts.TestConfig
{
    public interface IGameConfig
    {
        public Dictionary<Type, Type> Init();
    }
}