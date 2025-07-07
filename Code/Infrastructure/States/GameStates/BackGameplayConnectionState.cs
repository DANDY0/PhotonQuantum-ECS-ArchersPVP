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
    public class BackGameplayConnectionState: SimpleState
    {
        private readonly IGameStateMachine _stateMachine;
        private readonly PhotonConnector _photonConnector;
        private readonly IWindowService _windowService;
        private readonly IWindowFactory _windowFactory;

        public BackGameplayConnectionState(
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
                // _windowFactory.CreateStartConnection();
                _windowService.Close(WindowId.GameplayDebug);
                _windowService.Close(WindowId.MatchScore);
                _windowService.Close(WindowId.GameOver);

                Debug.Log("Entered StartConnectionState state");
                _windowService.Open(WindowId.LoadingBackFromGameplay);
                await _photonConnector.Connect();
                //any other initialization. SDK, plugins etc...
                _windowService.Close(WindowId.LoadingBackFromGameplay);

                _stateMachine.Enter<MenuState>();
            }
            catch (Exception e)
            {
                throw; // TODO handle exception
            }
        }
    }
}