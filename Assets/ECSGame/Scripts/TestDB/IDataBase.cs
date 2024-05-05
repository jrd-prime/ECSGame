using ECSGame.Scripts.State.Loading;

namespace ECSGame.Scripts.TestDB
{
    public interface IDataBase : ILoadable
    {
        string ILoadable.Description => "Data Base";

        public void ShowInfo();
    }
}