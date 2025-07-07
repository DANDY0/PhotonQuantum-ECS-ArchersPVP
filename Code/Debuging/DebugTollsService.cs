using Code.Gameplay.StaticData;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.UI.Gameplay.Windows.GameplayDebug;
using Code.UI.Windows;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;
using Zenject;

namespace Code.Debuging
{
    public class DebugToolsService : IDebugToolsService, ITickable
    {
        private readonly IUnityStaticDataService _unityStaticDataService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly IWindowService _windowService;
        
        public DebugSettings DebugSettings => _unityStaticDataService.DebugSettings;

        public DebugToolsService(
            IUnityStaticDataService unityStaticDataService,
            IGameStateMachine gameStateMachine,
            IWindowService windowService)
        {
            _unityStaticDataService = unityStaticDataService;
            _gameStateMachine = gameStateMachine;
            _windowService = windowService;
        }
        
        public void Tick()
        {
            if(_gameStateMachine.ActiveState is not BattleState)
                return;
            
            if (DebugSettings.AllowOpenGameOverCode && UnityEngine.Input.GetKeyDown(KeyCode.Escape))
            {
                if (_windowService.IsWindowOpen(WindowId.GameplayDebug))
                    _windowService.Close(WindowId.GameplayDebug);
                else
                    _windowService.Open(WindowId.GameplayDebug);
            }
        }
    }
}