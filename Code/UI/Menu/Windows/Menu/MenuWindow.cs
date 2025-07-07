using Code.Menu.PlayerAppearance;
using Code.UI.Menu.Windows;
using Code.UI.Windows;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Menu.Windows.Menu
{
    public class MenuWindow : BaseWindow
    {
        [SerializeField] private PlayerAppearancePanel _playerAppearancePanel;
        [SerializeField] private Button _quickPlayButton;
        [SerializeField] private Button _abilitiesButton;
        [SerializeField] private Toggle _addDummyToggle;
        [SerializeField] private MenuAbilitiesSelection _abilitiesPanel;

        public PlayerAppearancePanel PlayerAppearancePanel => _playerAppearancePanel;
        public Button QuickPlayButton => _quickPlayButton;
        public Button AbilitiesButton => _abilitiesButton;
        public Toggle AddDummyToggle => _addDummyToggle;
        public override WindowId Id => WindowId.Menu;

        protected override void Initialize()
        {
            _abilitiesPanel.Init();
        }
        
        public void OpenAbilities()
        {
            _abilitiesPanel.gameObject.SetActive(true);
        }
    }
}