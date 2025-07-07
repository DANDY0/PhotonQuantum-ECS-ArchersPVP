using Code.Infrastructure.Loading;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class LoadingBattleState : SimpleState
    {
        private readonly IGameStateMachine _gameStateMachine;
        private readonly ISceneLoader _sceneLoader;
        private readonly BattleSimulationLoader _battleSimulationLoader;

        public LoadingBattleState(ISceneLoader sceneLoader, BattleSimulationLoader battleSimulationLoader, IGameStateMachine gameStateMachine)
        {
            _sceneLoader = sceneLoader;
            _battleSimulationLoader = battleSimulationLoader;
            _gameStateMachine = gameStateMachine;
        }

        public override void Enter()
        {
            Debug.Log("Entered LoadingBattleState");
            _battleSimulationLoader.HandleRoomFull();
            // _sceneLoader.LoadScene(BATTLE_SCENE_NAME,
                // () => _gameStateMachine.Enter<BattleState>());
        }
    }
}