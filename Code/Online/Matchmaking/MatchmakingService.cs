using System;
using Code.Connection;
using Code.Connection.Code.Connection;
using Code.Infrastructure.Loading;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Online.Data;
using Code.UI.Windows;
using Photon.Realtime;
using UnityEngine;
using Zenject;

namespace Code.Online.Matchmaking
{
    public class MatchmakingService : IMatchmakingService, IInitializable, IDisposable
    {
        private Matchmaker _matchmaker;
        private RoomWatcher _roomWatcher;
        private PhotonRoomEventHandler _photonRoomEventHandler;
        private BattleSimulationLoader _sceneLoader;

        private IWindowService _windowService;
        private IGameStateMachine _stateMachine;
        private readonly PhotonClientArgsProvider _photonClientArgsProvider;
        private RealtimeClient _client;

        public event Action<string> OnMatchmakingFailed;

        public MatchmakingService(
            IWindowService windowService,
            IGameStateMachine stateMachine,
            PhotonClientArgsProvider photonClientArgsProvider,
            Matchmaker matchmaker,
            RoomWatcher roomWatcher,
            PhotonRoomEventHandler photonRoomEventHandler,
            BattleSimulationLoader sceneLoader)
        {
            _windowService = windowService;
            _stateMachine = stateMachine;
            _photonClientArgsProvider = photonClientArgsProvider;
            _matchmaker = matchmaker;
            _roomWatcher = roomWatcher;
            _sceneLoader = sceneLoader;
            _photonRoomEventHandler = photonRoomEventHandler;
        }

        public void Initialize()
        {
            UnityEngine.Debug.Log("Matchmaking SERVICE initialized");
            _roomWatcher.OnRoomFull += HandleRoomFull;
            _matchmaker.OnMatchmakingSuccess += HandleMatchSuccess;
            _matchmaker.OnMatchmakingFailed += MatchmakingFailed;
            _sceneLoader.BattleSimulationStartedEvent += BattleSimulationStartedHandler;
            _photonClientArgsProvider.RealtimeClientSetEvent += RealtimeClientSetHandler;
        }

        private void MatchmakingFailed(string msg)
        {
            OnMatchmakingFailed?.Invoke(msg);
        }

        public void Dispose()
        {
            _roomWatcher.OnRoomFull -= HandleRoomFull;
            _matchmaker.OnMatchmakingSuccess -= HandleMatchSuccess;
            _sceneLoader.BattleSimulationStartedEvent -= BattleSimulationStartedHandler;
            _matchmaker.OnMatchmakingFailed -= MatchmakingFailed;
            _photonClientArgsProvider.RealtimeClientSetEvent -= RealtimeClientSetHandler;
        }

        public void StartMatchmaking()
        {
            _matchmaker.StartMatchmaking();
        }

        private void RealtimeClientSetHandler(RealtimeClient client) => _client = client;

        private void HandleMatchSuccess()
        {
            Logger.Log("[MatchmakingService] Match found, waiting for room fill...");
        }

        private void HandleRoomFull()
        {
            Logger.Log("[MatchmakingService] Room full. Starting to load game scene...");
            _stateMachine.Enter<LoadingBattleState>();
        }
        
        private void BattleSimulationStartedHandler()
        {
            if (_client.LocalPlayer.IsMasterClient)
            {
                Logger.Log("Building MatchIntroData as master...");

                var matchIntroData = new MatchSyncData();

                foreach (var player in _client.CurrentRoom.Players.Values)
                {
                    var props = player.CustomProperties;

                    props.TryGetValue(nameof(MatchPlayerDataType.UserId), out var userId);
                    props.TryGetValue(nameof(MatchPlayerDataType.Nickname), out var nickname);

                    var playerData = new MatchPlayerData()
                    {
                        UserId = userId?.ToString() ?? player.UserId,
                        Nickname = nickname?.ToString() ?? player.NickName,
                    };

                    matchIntroData.Players.Add(playerData);
                }

                foreach (var p in matchIntroData.Players) 
                    Logger.Log($"[MatchIntroData] UserId: {p.UserId}, Nickname: {p.Nickname}");

                _photonRoomEventHandler.SendMatchIntroData(matchIntroData);
            }
        }

    }
}