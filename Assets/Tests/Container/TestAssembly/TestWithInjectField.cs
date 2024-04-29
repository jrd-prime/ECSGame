using ECSGame.Scripts.Core.Annotation;
using ECSGame.Scripts.Core.DI.Interface;

namespace Tests.Container.TestAssembly
{
    public class TestWithInjectField
    {
        [JInject] private IMyContainer _containerFull;
        public IMyContainer GetContainer() => _containerFull;
    }
}