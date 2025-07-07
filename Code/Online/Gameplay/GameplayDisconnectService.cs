using System.Threading.Tasks;
using Code.Connection;
using Code.Infrastructure.Loading;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.UI.Windows;
using Photon.Realtime;
using Quantum;

namespace Code.Online.Gameplay
{
    public class GameplayDisconnectService : IGameplayDisconnectService
    {
        private readonly IPhotonConnectionService _photonConnectionService;
        private readonly ISceneLoader _sceneLoader;
        private readonly IWindowService _windowService;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly Matchmaker _matchmaker;

        public GameplayDisconnectService(
            IPhotonConnectionService photonConnectionService,
            ISceneLoader sceneLoader,
            IWindowService windowService,
            IGameStateMachine gameStateMachine,
            Matchmaker matchmaker
        )
        {
            _photonConnectionService = photonConnectionService;
            _sceneLoader = sceneLoader;
            _windowService = windowService;
            _gameStateMachine = gameStateMachine;
            _matchmaker = matchmaker;
        }

        public void GoMenu()
        {
            _ = HandleDisconnectionFromGameplay();
        }

        private async Task HandleDisconnectionFromGameplay()
        {
            if (QuantumRunner.Default != null)
                QuantumRunner.Default.Shutdown();

            await _matchmaker.LeaveRoomAsync();

            await _photonConnectionService.Client.DisconnectAsync();

            await _sceneLoader.UnloadSceneAsync(SceneId.Battle);

            _gameStateMachine.Enter<BackGameplayConnectionState>();
        }
    }
}