using Sources.Scripts.Annotation;
using Sources.Scripts.DI;
using Sources.Scripts.DI.Interface;

namespace Tests.Container.TestAssembly
{
    public class TestWithInjectField
    {
        [JInject] private IMyContainer _containerFull;
        public IMyContainer GetContainer() => _containerFull;
    }
}