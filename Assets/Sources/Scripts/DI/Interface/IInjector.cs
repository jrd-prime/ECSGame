using System.Reflection;
using System.Threading.Tasks;

namespace Sources.Scripts.DI.Interface
{
    public interface IInjector: IFieldsInjectable
    {
        Task InjectDependenciesAsync(Assembly assembly);
    }
}