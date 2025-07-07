using System;
using Code.UI.Gameplay.Windows.GameplayDebug;
using Code.UI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.Debuging
{
    public class GameplayDebugButton: MonoBehaviour, IInitializable, IDisposable
    {
        [SerializeField] private Button _gameplayDebugButton;
        
        [Inject] private IWindowService _windowService;
        [Inject] private IDebugToolsService _debugToolsService;

        public void Initialize()
        {
            if (_debugToolsService.DebugSettings.ShowDebugButton) 
                _gameplayDebugButton.onClick.AddListener(OpenDebugWindow);
        }

        public void Dispose()
        {
            if (_debugToolsService.DebugSettings.ShowDebugButton) 
                _gameplayDebugButton.onClick.RemoveListener(OpenDebugWindow);
        }

        private void OpenDebugWindow()
        {
            _windowService.Open(WindowId.GameplayDebug);
        }
    }
}