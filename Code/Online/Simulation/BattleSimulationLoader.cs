using System;
using System.Threading;
using System.Threading.Tasks;
using Code.Connection;
using Code.Connection.Code.Connection;
using Code.UI.Windows;
using Photon.Deterministic;
using Quantum;
using Quantum.Menu;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Code.Infrastructure.Loading
{
public class BattleSimulationLoader
{
    public event Action<Exception> OnLoadFailed;
    public event Action BattleSimulationStartedEvent;

    private readonly PhotonClientArgsProvider _photonClientArgsProvider;
    private readonly PhotonRoomEventHandler _photonRoomEventHandler;
    private readonly ISceneLoader _sceneLoader;
    private readonly IWindowFactory _windowFactory;

    private QuantumRunner _runner;

    private bool _isLoading;

    public BattleSimulationLoader(
        PhotonRoomEventHandler photonRoomEventHandler,
        ISceneLoader sceneLoader,
        IWindowFactory windowFactory,
        PhotonClientArgsProvider photonClientArgsProvider)
    {
        _photonRoomEventHandler = photonRoomEventHandler;
        _sceneLoader = sceneLoader;
        _windowFactory = windowFactory;
        _photonClientArgsProvider = photonClientArgsProvider;
    }
    
    public void HandleRoomFull()
    {
        if (_isLoading) 
            return;
        _isLoading = true;

        LoadGameScene(() =>
        {
            
            StartQuantumRunnerAsync().ContinueWith(task =>
            {
                if (task.Exception != null)
                {
                    UnityEngine.Debug.LogError($"Failed to start Quantum: {task.Exception.InnerException}");
                    OnLoadFailed?.Invoke(task.Exception.InnerException);
                    return;
                }

                BattleSimulationStartedEvent?.Invoke();
            }, TaskScheduler.FromCurrentSynchronizationContext());
        });
    }

    private void LoadGameScene(Action onLoaded)
    {
        _sceneLoader.LoadSceneAdditive(SceneId.Battle, () =>
        {
            SceneManager.SetActiveScene(SceneManager.GetSceneByName(nameof(SceneId.Battle)));
            UnityEngine.Debug.Log($"[GameSceneLoader] Scene '{SceneId.Battle}' loaded.");
            onLoaded?.Invoke();
        });
    }

    private async Task StartQuantumRunnerAsync()
    {
        var args = _photonClientArgsProvider.ConnectArgs;

        if (args == null)
        {
            throw new InvalidOperationException("ConnectArgs is null! Cannot start Quantum Runner.");
        }

        var sessionRunnerArguments = new SessionRunner.Arguments
        {
            RunnerFactory = QuantumRunnerUnityFactory.DefaultFactory,
            GameParameters = QuantumRunnerUnityFactory.CreateGameParameters,
            ClientId = !string.IsNullOrEmpty(args.QuantumClientId) ? args.QuantumClientId : _photonClientArgsProvider.Client.UserId,
            RuntimeConfig = args.RuntimeConfig,
            SessionConfig = args.SessionConfig?.Config,
            GameMode = DeterministicGameMode.Multiplayer,
            PlayerCount = 2,
            Communicator = new QuantumNetworkCommunicator(_photonClientArgsProvider.Client),
            CancellationToken = new CancellationToken(),
            RecordingFlags = args.RecordingFlags,
            InstantReplaySettings = args.InstantReplaySettings,
            DeltaTimeType = args.DeltaTimeType,
            StartGameTimeoutInSeconds = args.StartGameTimeoutInSeconds,
            GameFlags = args.GameFlags,
            OnShutdown = OnSessionShutdown,
        };

        _runner = (QuantumRunner)await SessionRunner.StartAsync(sessionRunnerArguments);

        if (args.RuntimePlayers != null)
        {
            for (int i = 0; i < args.RuntimePlayers.Length; i++)
                _runner.Game.AddPlayer(i, args.RuntimePlayers[i]);
        }
    }

    private void OnSessionShutdown(ShutdownCause cause, SessionRunner runner)
    {
        Debug.LogError("[GameSceneLoader] Session shutdown.");
        _isLoading = false; 
    }

}
}
