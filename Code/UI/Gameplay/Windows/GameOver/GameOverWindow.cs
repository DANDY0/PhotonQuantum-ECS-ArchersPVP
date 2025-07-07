using System;
using Code.UI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Gameplay.Windows.GameOver
{
    public class GameOverWindow : BaseWindow
    {
        [SerializeField] private Button _goMenuButton;
        [SerializeField] private TextMeshProUGUI _winnerNickname;

        public override WindowId Id => WindowId.GameOver;

        private Action _goBackCallback;

        public void SetWinner(string winnerNickname)
        {
            _winnerNickname.text = winnerNickname;
        }

        public override void Open()
        {
            base.Open();
            SetInteractable(true);
        }

        public void SubscribeToGoBack(Action callback)
        {
            _goMenuButton.onClick.RemoveListener(OnGoBackClicked);
            
            _goBackCallback = callback;
            _goMenuButton.onClick.AddListener(OnGoBackClicked);
        }

        public void UnSubscribeToGoBack()
        {
            _goMenuButton.onClick.RemoveListener(OnGoBackClicked);
            _goBackCallback = null;
        }

        private void OnGoBackClicked()
        {
            _goBackCallback?.Invoke();
        }

        public void SetInteractable(bool state)
        {
            _goMenuButton.interactable = state;
        }
    }
}