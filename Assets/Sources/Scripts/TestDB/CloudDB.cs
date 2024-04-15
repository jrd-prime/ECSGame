using UnityEngine;

namespace Sources.Scripts.TestDB
{
    public class CloudDB : IDataBase
    {
        public void ShowInfo()
        {
            Debug.LogWarning("Its cloud DB");
        }
    }
}