using System;
using Code.Gameplay.StaticData;
using Code.Infrastructure.Loading;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class BootstrapState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly IStaticDataService _staticDataService;
        private readonly ISceneLoader _sceneLoader;
        private readonly IUnityStaticDataService _unityStaticDataService;

        public BootstrapState(
            IGameStateMachine stateMachine,
            IStaticDataService staticDataService,
            ISceneLoader sceneLoader,
            IUnityStaticDataService unityStaticDataService)
        {
            _stateMachine = stateMachine;
            _staticDataService = staticDataService;
            _sceneLoader = sceneLoader;
            _unityStaticDataService = unityStaticDataService;
        }

        public override void Enter()
        {
            try
            {
                Debug.Log("Entered BootstrapState state");
                _staticDataService.LoadAll();
                _unityStaticDataService.LoadAll();

                _sceneLoader.LoadScene(SceneId.StartConnection,
                    () => _stateMachine.Enter<StartConnectionState>());
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }
    }
}