using System.Reflection;
using System.Threading.Tasks;
using ECSGame.Scripts.Core.Config.Interface;
using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.Core.DI.Interface
{
    public interface IMyContainer: IFieldsInjectable, ILoadable

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