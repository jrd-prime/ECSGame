using System.Reflection;
using System.Threading.Tasks;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface IInjector: IFieldsInjectable
    {
        Task InjectDependenciesAsync(Assembly assembly);
    }
}