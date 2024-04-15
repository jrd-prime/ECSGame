using System;
using Sources.Scripts.Annotation;
using UnityEngine;

namespace Sources.Scripts.TestDB
{
    public class DBController : MonoBehaviour
    {
        [JInject] private IDataBase _dataBase;

        private void Start()
        {
            _dataBase.ShowInfo();
        }
    }
}