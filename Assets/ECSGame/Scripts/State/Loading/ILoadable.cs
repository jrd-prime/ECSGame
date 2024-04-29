using Cysharp.Threading.Tasks;

namespace ECSGame.Scripts.State.Loading
{
    public interface ILoadable
    {
        public UniTask Load();
    }
}