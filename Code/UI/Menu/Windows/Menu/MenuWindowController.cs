using UnityEngine;
using Code.Connection;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Menu.PlayerAppearance;
using Code.UI.Menu.Windows.Menu;
using Code.UI.Windows;
using Zenject;

namespace Code.Menu
{
    public class MenuWindowController: BaseController<MenuWindow>, IInitializable
    {
        private const string IS_DUMMY_PREF_KEY = "IsDummy";

        private readonly IGameStateMachine _gameStateMachine;
        private readonly PhotonClientArgsProvider _photonClientArgsProvider;
    
        private PlayerAppearanceController _appearanceController;

        public MenuWindowController(
            IGameStateMachine gameStateMachine,
            PhotonClientArgsProvider photonClientArgsProvider)
        {
            _gameStateMachine = gameStateMachine;
            _photonClientArgsProvider = photonClientArgsProvider;
        }

        public void Initialize()
        {
            View.QuickPlayButton.onClick.AddListener(OnQuickPlayClicked);
            View.AbilitiesButton.onClick.AddListener(OnAbilityClicked);
            View.AddDummyToggle.onValueChanged.AddListener(OnAddDummyToggleChanged);

            bool isDummy = PlayerPrefs.GetInt(IS_DUMMY_PREF_KEY, 0) == 1;
            View.AddDummyToggle.isOn = isDummy;
            _photonClientArgsProvider.RuntimeLocalPlayer.createDummy = isDummy;

            if (_appearanceController != null)
                return;

            var model = new PlayerAppearanceModel();
            _appearanceController = new PlayerAppearanceController(
                model,
                View.PlayerAppearancePanel,
                _photonClientArgsProvider);
            _appearanceController.Open();
        }
        
        private void OnAbilityClicked()
        {
            View.OpenAbilities();
        }

        private void OnQuickPlayClicked()
        {
            _gameStateMachine.Enter<MatchmakingState>();
        }

        private void OnAddDummyToggleChanged(bool isOn)
        {
            _photonClientArgsProvider.RuntimeLocalPlayer.createDummy = isOn;
            PlayerPrefs.SetInt(IS_DUMMY_PREF_KEY, isOn ? 1 : 0);
            PlayerPrefs.Save();
        }
    }
}