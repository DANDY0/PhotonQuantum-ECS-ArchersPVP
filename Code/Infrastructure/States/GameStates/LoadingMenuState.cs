using Code.Infrastructure.Loading;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Code.UI.Windows;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class LoadingMenuState : SimpleState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly IWindowService _windowService;

        public LoadingMenuState(
            ISceneLoader sceneLoader,
            IWindowService windowService,
            IGameStateMachine gameStateMachine)
        {
            _sceneLoader = sceneLoader;
            _windowService = windowService;
            _gameStateMachine = gameStateMachine;
        }

        public override void Enter()
        {
            Debug.Log("Entered LoadingMenuState");

            _windowService.Close(WindowId.LoadingConnectionToMaster);
            _sceneLoader.LoadScene(SceneId.Menu,
                () => _gameStateMachine.Enter<MenuState>());
        }
    }
}