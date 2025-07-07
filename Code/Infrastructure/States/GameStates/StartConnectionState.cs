using System;
using Code.Connection;
using Code.Gameplay.StaticData;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Infrastructure.States.StateMachine;
using Code.UI.Windows;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class StartConnectionState : SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly PhotonConnector _photonConnector;
        private readonly IWindowService _windowService;
        private readonly IWindowFactory _windowFactory;

        public StartConnectionState(
            PhotonConnector photonConnector,
            IGameStateMachine stateMachine,
            IWindowService windowService,
            IWindowFactory windowFactory)
        {
            _photonConnector = photonConnector;
            _stateMachine = stateMachine;
            _windowService = windowService;
            _windowFactory = windowFactory;
        }

        public override async void Enter()
        {
            try
            {
                _windowFactory.CreateStartConnection();


                Debug.Log("Entered StartConnectionState state");
                _windowService.Open(WindowId.LoadingConnectionToMaster);
                await _photonConnector.Connect();
                //any other initialization. SDK, plugins etc...

                _stateMachine.Enter<LoadingMenuState>();
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }
    }
}