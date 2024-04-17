using System.Threading.Tasks;
using Sources.Scripts.Core;
using Sources.Scripts.DI;

namespace Sources.Scripts.TestConfig
{
    public class TestBinds : IBindsConfig
    {
        public async Task InitBindings(Container container)
        {
            await container.BindInterface<IServiceConfig, NetServiceConfig>();
            await container.BindSelfAsync<TestManager>();
        }
    }
}