using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Code.UI.Windows;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class BattleState: SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IWindowService _windowService;

        public BattleState(
            IGameStateMachine stateMachine,
            IWindowService windowService)
        {
            _stateMachine = stateMachine;
            _windowService = windowService;
        }

        public override void Enter()
        {
            Debug.Log("Entered BattleState state");
            // _windowService.Close(WindowId.LoadingMatchmaking);
            // _windowService.Close(WindowId.Menu);
        }  
    }
}