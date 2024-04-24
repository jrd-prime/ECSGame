using Sources.Scripts.Annotation;
using Sources.Scripts.DI;

namespace Tests.Container.TestAssembly
{
    public class TestWithInjectField
    {
        [JInject] private MyContainer _container;
        public MyContainer GetContainer() => _container;
    }
}