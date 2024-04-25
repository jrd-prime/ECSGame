using System.Reflection;
using System.Threading.Tasks;

namespace Sources.Scripts.DI.Interface
{
    public interface IContainerInjector
    {
        Task InjectDependenciesAsync(Assembly assembly);
    }
}