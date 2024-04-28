using System.Reflection;
using System.Threading.Tasks;
using Sources.Scripts.Core.Config;

namespace Sources.Scripts.DI.Interface
{
    public interface IMyContainer: IFieldsInjectable

    {
    public Task InjectServicesAsync(Assembly assembly);
    public T GetService<T>() where T : class;
    public void Bind<T>() where T : class;
    public void Bind<TBase, TImpl>() where TBase : class where TImpl : TBase;
    public void Bind<T>(in object implInstance) where T : class;
    public void Bind<TBase, TImpl>(in TImpl implInstance) where TBase : class where TImpl : TBase;
    public void BindConfig(in IBindableConfiguration configuration);
    }
}