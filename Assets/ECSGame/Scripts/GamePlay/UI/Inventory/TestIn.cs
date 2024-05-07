using ECSGame.Scripts.Core.DataBase;
using ECSGame.Scripts.Core.DI.Interface;
using UnityEngine;

namespace ECSGame.Scripts.GamePlay.UI.Inventory
{
    public class TestIn : MonoBehaviour
    {
        private readonly IMyContainer _container = AppContext.Instance.Container;

        private void Start()
        {
            Debug.LogWarning(_container.GetService<IDataBase>());
            Debug.LogWarning($"Container??? = {_container}");
        }
    }
}