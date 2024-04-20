using Moq;
using Sources.Scripts.Core;
using Sources.Scripts.DI;

namespace Tests.DITests
{
    public static class ContainerTests
    {
        public static readonly Container Container = new();
        public static readonly TestObject TestObjectInstance = new();
        public static readonly Mock<IBindableConfig> BindableConfigWithRecord = new();
        public static readonly Mock<IBindableConfig> BindableConfigWithEmptyDict = new();
    }
}