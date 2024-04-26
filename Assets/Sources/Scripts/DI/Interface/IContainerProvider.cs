using Cysharp.Threading.Tasks;

namespace Sources.Scripts.DI.Interface
{
    public interface IContainerProvider
    {
        public UniTask<IMyContainer> GetContainer();
    }
}