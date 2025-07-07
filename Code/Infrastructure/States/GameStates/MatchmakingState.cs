using Code.Connection;
using Code.Infrastructure.Loading;
using Code.Infrastructure.States.StateInfrastructure;
using Code.UI.Windows;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class MatchmakingState : SimpleState
    {
        private readonly ISceneLoader _sceneLoader;
        private readonly IMatchmakingService _matchmakingService;
        private readonly IWindowService _windowService;

        public MatchmakingState(
            IMatchmakingService matchmakingService,
            IWindowService windowService,
            ISceneLoader sceneLoader)
        {
            _matchmakingService = matchmakingService;
            _windowService = windowService;
            _sceneLoader = sceneLoader;
        }

        public override void Enter()
        {
            _matchmakingService.StartMatchmaking();
            _windowService.Open(WindowId.LoadingMatchmaking);
            Debug.Log("Entered MatchmakingState");
        }

        protected override void Exit()
        {
            Debug.Log("Exit MatchmakingState");
        }
    }
}