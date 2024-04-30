using System;
using System.Diagnostics;
using Cysharp.Threading.Tasks;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.UIElements;
using UnityEngine.UIElements.Experimental;
using Debug = UnityEngine.Debug;

namespace ECSGame.Scripts.Core.AssetLoader
{
    public class LocalAssetLoader : IAssetLoader
    {
        private GameObject _cachedObject;

        public async UniTask<T> Load<T>(string assetName, Transform parent = null)
        {
            var handle = Addressables.InstantiateAsync(assetName, parent);
            _cachedObject = await handle.Task;
            if (_cachedObject.TryGetComponent(out T component) == false)
                throw new NullReferenceException($"Object of type {typeof(T)} is null on " +
                                                 "attempt to load it from addressables");
            return component;
        }
        // public override void ShowPanel()
        // {
        //     base.ShowPanel();
        //     Panel.experimental.animation
        //         .Start(
        //             new StyleValues { bottom = PanelHeight * -1 },
        //             new StyleValues { bottom = BottomMargin },
        //             ShowDuration)
        //         .Ease(Easing.OutElastic)
        //         .KeepAlive();
        // }
        //
        // public override void HidePanel()
        // {
        //     Panel.experimental.animation
        //         .Start(
        //             new StyleValues { bottom = BottomMargin },
        //             new StyleValues { bottom = PanelHeight * -1 },
        //             HideDuration)
        //         .Ease(Easing.InQuad)
        //         .KeepAlive()
        //         .onAnimationCompleted = () => base.HidePanel();
        // }

        public void Unload()
        {
            if (_cachedObject == null)
                return;

            Debug.LogWarning(_cachedObject);
            var a = _cachedObject.gameObject.GetComponent<UIDocument>().rootVisualElement;

            Debug.LogWarning(a.name);

            Debug.LogWarning(a[0].name);
            var b = a.Q<VisualElement>("loading-screen-root");

            var height = b.layout.height;
            var width = b.layout.width;

            b.experimental.animation.Start(
                from: new StyleValues
                {
                    opacity = 1f,
                    width = width,
                    height = height
                },
                to: new StyleValues
                {
                    opacity = 0f,
                    width = width * 1.05f,
                    height = height * 1.05f
                },
                durationMs: 1000).onAnimationCompleted = () =>
            {
                _cachedObject.SetActive(false);
                Addressables.ReleaseInstance(_cachedObject);
                _cachedObject = null;
            };
        }
    }
}