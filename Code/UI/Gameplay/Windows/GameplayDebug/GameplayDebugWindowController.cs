using System;
using Code.Online.Gameplay;
using Code.UI.Windows;
using Zenject;

namespace Code.UI.Gameplay.Windows.GameplayDebug
{
    public class GameplayDebugWindowController: BaseController<GameplayDebugWindow>, 
        IGameplayDebugWindowController,
        IInitializable,
        IDisposable
    {
        private readonly IGameplayDisconnectService _gameplayDisconnectService;

        public GameplayDebugWindowController(
            IGameplayDisconnectService gameplayDisconnectService)
        {
            _gameplayDisconnectService = gameplayDisconnectService;
        }
        
        public void Initialize()
        {
            View.SubscribeButton(View.DisconnectButton, DisconnectFromGameplay);
            View.SubscribeButton(View.CloseButton, View.Close);
        }

        public void Dispose()
        {
            View.UnSubscribeButton(View.DisconnectButton);
            View.UnSubscribeButton(View.CloseButton);
        }

        public void OpenDevToolsWindow()
        {
            View.Open();
        }

        private void DisconnectFromGameplay()
        {
            _gameplayDisconnectService.GoMenu();
        }
    }
}