using System;
using System.Collections.Generic;
using Sources.Scripts.TestDB;
using Sources.Scripts.TestViews;

namespace Sources.Scripts.TestConfig
{
    public class NetGameConfig : IGameConfig
    {
        public Dictionary<Type, Type> Init()
        {
            return new Dictionary<Type, Type>()
            {
                { typeof(IDataBase), typeof(CloudDB) },
                { typeof(IViews), typeof(OldStyleView) }
            };
        }
    }
}