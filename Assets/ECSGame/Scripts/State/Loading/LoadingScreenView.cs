using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.UIElements;

namespace ECSGame.Scripts.State.Loading
{
    public class LoadingScreenView : MonoBehaviour
    {
        private Action<ILoadable> _onLoadingOperation;
        private Label _text;
        private VisualElement _pb;
        private float _pbFullVal;
        public int _steps;
        private int maxbar = 600;
        private int _tempStep;

        private void Awake()
        {
            _onLoadingOperation += OnLoadingOperation;
            var root = gameObject.GetComponent<UIDocument>().rootVisualElement;
            _text = root.Q<Label>("foot-text-label");
            _pb = root.Q<VisualElement>("pb-bar");
            _pbFullVal = _pb.style.width.value.value;

            _pb.style.width = 0;
        }

        private void Start()
        {
            _pb.style.width = 70;
        }

        private void OnLoadingOperation(ILoadable obj)
        {
            _text.text = obj.Description;

            //TODO remove or rework
            {
                if (_tempStep == 0)
                {
                    _pb.style.width = 0;
                    _tempStep += 1;
                }
                else if (_tempStep == _steps)
                {
                    _pb.style.width = maxbar;
                    _tempStep = 0;
                }
                else
                {
                    _pb.style.width = maxbar / _steps * _tempStep;
                    _tempStep += 1;
                }
            }
        }


        public async UniTask Load(Loader loader)
        {
            foreach (var operation in loader.LoadingQueue)
            {
                await operation.Load(_onLoadingOperation);
                Debug.LogWarning($"start bonus delay {loader.LoadingQueueDelay[operation]}");
                await UniTask.Delay(loader.LoadingQueueDelay[operation]);
            }
        }
    }
}