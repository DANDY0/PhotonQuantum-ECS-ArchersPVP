using System;
using Code.Online.Gameplay;
using Code.UI.Windows;
using UnityEngine;
using Zenject;

namespace Code.UI.Gameplay.Windows.GameOver
{
    public class GameOverWindowController : BaseController<GameOverWindow>, IGameOverWindowController, IInitializable, IDisposable
    {
        private readonly IGameplayDisconnectService _gameplayDisconnectService;
        private bool _isWindowOpen;

        public GameOverWindowController(
            IGameplayDisconnectService gameplayDisconnectService)
        {
            _gameplayDisconnectService = gameplayDisconnectService;
        }

        public void Initialize()
        {
            View.SubscribeToGoBack(GoMenu);
            _isWindowOpen = false;
        }

        public void Dispose()
        {
            View.UnSubscribeToGoBack();
        }

        private async void GoMenu()
        {
            View.SetInteractable(false);
            _gameplayDisconnectService.GoMenu();
        }

        public void SetWinner(string winnerNickname)
        {
            View.SetWinner(winnerNickname);
        }
    }
}