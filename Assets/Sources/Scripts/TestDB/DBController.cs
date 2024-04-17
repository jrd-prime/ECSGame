using System;
using Sources.Scripts.Annotation;
using UnityEngine;

namespace Sources.Scripts.TestDB
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