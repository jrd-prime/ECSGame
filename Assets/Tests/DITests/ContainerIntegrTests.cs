using Sources.Scripts.DI;

namespace Tests.DITests
{
    public class ContainerIntegrTests
    {
        private readonly Container _container = ContainerTests.Container;
        private readonly TestObject _testObjectInstance = ContainerTests.TestObjectInstance;
    }
}