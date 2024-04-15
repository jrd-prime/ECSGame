using System;
using Sources.Scripts.Context;
using Sources.Scripts.Factory;
using UnityEngine;

namespace Sources.Scripts
{
    [RequireComponent(typeof(GameRoot))]
    public class GameRoot : MonoBehaviour
    {
        private void Awake()
        {
            DontDestroyOnLoad(gameObject);
        }

        // public ApplicationContext Run()
        // {
        //     ApplicationContext applicationContext = new ApplicationContext();
        //     ServiceFactory serviceFactory = new ServiceFactory(applicationContext);
        //     applicationContext.SetServiceFactory(serviceFactory);
        //
        //     return applicationContext;
        // }
        //
        // private void Start()
        // {
        //     GameRoot app = gameObject.GetComponent<GameRoot>();
        //     ApplicationContext applicationContext = app.Run();
        // }
    }
}