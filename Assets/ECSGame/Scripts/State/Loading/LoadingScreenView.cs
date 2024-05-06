using System;
using System.Collections;
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
        private const float BarLenght = 600f;
        private float _time = -1;
        private int _tempStep;
        private Label _pbLabel;


        private void Awake()
        {
            _onLoadingOperation += OnLoadingOperation;
            var root = gameObject.GetComponent<UIDocument>().rootVisualElement;
            _text = root.Q<Label>("foot-text-label");
            _pb = root.Q<VisualElement>("pb-bar");
            _pbLabel = root.Q<Label>("pb-bar-label");


            _text.text = "";
            _pb.style.width = 0;
            _pbLabel.text = "0 %";
        }

        private IEnumerator UpdateProgressBar()
        {
            var timeInSec = _time / 1000;
            var pxPerSec = BarLenght / timeInSec;
            const float onePercentOfBar = BarLenght / 100;

            while (timeInSec > 0)
            {
                var dt = Time.deltaTime;
                var oldVal = _pb.style.width.value.value;
                var newVal = oldVal + (dt * pxPerSec);

                if (_pb.style.width.value.value < BarLenght)
                {
                    _pbLabel.text = Mathf.Round(newVal / onePercentOfBar) + " %";
                    _pb.style.width = _pb.style.width.value.value + (dt * pxPerSec);
                }

                timeInSec -= dt;
                yield return null;
            }
        }


        private void OnLoadingOperation(ILoadable obj)
        {
            var prefix = "Loading: ";
            var postfix = "...";
            _text.text = prefix + obj.Description + postfix;
        }


        public async UniTask Load(Loader loader)
        {
            StartCoroutine(UpdateProgressBar());

            foreach (var operation in loader.LoadingQueue)
            {
                var fakeDelay = UniTask.Delay(loader.LoadingQueueDelay[operation]);
                await UniTask.WhenAll(
                    operation.Load(_onLoadingOperation),
                    fakeDelay);
            }
        }

        public void SetTime(int timeSum) => _time = timeSum;
    }
}