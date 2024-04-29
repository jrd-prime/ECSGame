using UnityEngine;

namespace ECSGame.Scripts.TestDB
{
    public class DBController 
    {
         private IDataBase _dataBase;

        private void Start()
        {
            _dataBase.ShowInfo();
        }

        public void show()
        {
            Debug.Log("SHOW SHOW");
        }
    }
}