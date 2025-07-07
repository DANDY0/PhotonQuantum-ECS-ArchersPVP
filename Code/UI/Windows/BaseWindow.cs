using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Windows
{
    public abstract class BaseWindow : MonoBehaviour
    {
        public abstract WindowId Id { get; }

        private readonly Dictionary<Button, UnityEngine.Events.UnityAction> _buttonCallbacks = new();

        private void Awake() =>
            OnAwake();

        private void Start()
        {
            Initialize();
            SubscribeUpdates();
        }

        private void OnDestroy() =>
            Cleanup();

        protected virtual void OnAwake()
        {
        }

        protected virtual void Initialize()
        {
        }

        protected virtual void SubscribeUpdates()
        {
        }

        protected virtual void UnsubscribeUpdates()
        {
        }

        protected virtual void Cleanup() =>
            UnsubscribeUpdates();

        public virtual void Open() =>
            gameObject.SetActive(true);

        public virtual void Close() =>
            gameObject.SetActive(false);

        /// <summary>
        /// Universal button subscribe 
        /// </summary>
        public void SubscribeButton(Button button, Action callback)
        {
            if (button == null || callback == null)
            {
                Debug.LogWarning($"[BaseWindow] SubscribeButton failed: null button or callback in {Id}");
                return;
            }

            UnSubscribeButton(button);

            UnityEngine.Events.UnityAction unityAction = () => callback();
            _buttonCallbacks[button] = unityAction;
            button.onClick.AddListener(unityAction);
        }

        public void UnSubscribeButton(Button button)
        {
            if (button is null)
                return;

            if (_buttonCallbacks.TryGetValue(button, out var unityAction))
            {
                button.onClick.RemoveListener(unityAction);
                _buttonCallbacks.Remove(button);
            }
        }
    }
}
