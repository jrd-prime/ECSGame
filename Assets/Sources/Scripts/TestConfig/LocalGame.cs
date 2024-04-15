using System;
using System.Collections.Generic;
using Sources.Scripts.TestDB;
using Sources.Scripts.TestViews;

namespace Sources.Scripts.TestConfig
{
    public class LocalGame : IGameConfig
    {
        public Dictionary<Type, Type> Init()
        {
            return new Dictionary<Type, Type>()
            {
                { typeof(IDataBase), typeof(LocalDB) },
                { typeof(IViews), typeof(ModernView) }
            };
        }
    }
}