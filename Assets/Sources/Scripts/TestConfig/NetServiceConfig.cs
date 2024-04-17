using System;
using System.Collections.Generic;
using Sources.Scripts.TestDB;
using Sources.Scripts.TestViews;

namespace Sources.Scripts.TestConfig
{
    public class NetServiceConfig : IServiceConfig
    {
        public Dictionary<Type, Type> GetServicesList()
            => new()
            {
                { typeof(IDataBase), typeof(CloudDB) },
                { typeof(IViews), typeof(OldStyleView) }
            };
    }
}