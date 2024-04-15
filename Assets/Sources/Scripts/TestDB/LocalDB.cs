using UnityEngine;

namespace Sources.Scripts.TestDB
{
    public class LocalDB : IDataBase
    {
        public void ShowInfo()
        {
            Debug.LogError("Its local DB");
        }
    }
}