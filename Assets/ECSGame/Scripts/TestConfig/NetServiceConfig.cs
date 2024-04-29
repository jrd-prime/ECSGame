using System;
using System.Collections.Generic;
using ECSGame.Scripts.TestDB;
using ECSGame.Scripts.TestViews;

namespace ECSGame.Scripts.TestConfig
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