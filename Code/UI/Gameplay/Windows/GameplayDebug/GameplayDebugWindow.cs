using Code.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Gameplay.Windows.GameplayDebug
{
    public class GameplayDebugWindow: BaseWindow
    {
        //window content
        [SerializeField] private Transform _contentRoot;
        [SerializeField] private Button _disconnectButton;
        [SerializeField] private Button _closeButton;
        
        public Button DisconnectButton => _disconnectButton;
        public Button CloseButton => _closeButton;

        public override WindowId Id => WindowId.GameplayDebug;
    }
}