using System;
using Sources.Scripts.Utils;
using UnityEngine;
using AppContext = Sources.Scripts.DI.AppContext;

namespace Sources.Scripts.Core.Loading
{
    public sealed class AppStart : MonoBehaviour
    {
        [SerializeField] private GameObject _appContextHolderGo;

        private AppContext _context;

        private void Awake() => _context = _appContextHolderGo.GetComponent<AppContext>();

        private async void Start()
        {
            await _context.InitializeAsync();

            JLog.Msg($"(Services initialization FINISHED...");

            Debug.LogWarning("STAAAART THHHE GAMEE");
        }

        private void OnValidate()
        {
            if (_appContextHolderGo == null)
                throw new Exception($"AppContextHolder not set to {gameObject.name}!");
        }
    }
}