using System;
using System.Linq;
using Sources.Scripts.Factory;
using Sources.Scripts.ServiceConfig;
using UnityEngine;

namespace Sources.Scripts
{
    public class TestMono : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            var a = ServiceFactory.Instance.GetService<IPresenters>();
            a.gogo();
            
            
            var b = ServiceFactory.Instance.GetService<IPresenters>();
            b.gogo();
        }

        // Update is called once per frame
        void Update()
        {
        }
    }
}