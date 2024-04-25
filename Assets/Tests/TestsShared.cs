using System;
using System.Collections.Generic;
using Sources.Scripts.Annotation;
using Sources.Scripts.Core.Config;
using Sources.Scripts.DI;

namespace Tests
{
    public class TestsShared
    {
    }

    public class TestObject
    {
    }

    public class TestBase
    {
    }

    public class TestImpl : TestBase
    {
    }

    public class TestObject1
    {
    }

    public class TestBase1
    {
    }

    public class TestImpl1 : TestBase1
    {
    }
    public class TestEmptyConfig : IConfiguration
    {
        public Dictionary<Type, Type> GetBindings()
        {
            return new Dictionary<Type, Type> { };
        }
    }

    public class TestConfigWithRecord : IConfiguration
    {
        public Dictionary<Type, Type> GetBindings()
        {
            return new Dictionary<Type, Type> { { typeof(TestBase), typeof(TestImpl) } };
        }
    }


}