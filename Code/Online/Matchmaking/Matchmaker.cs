using System;
using System.Threading;
using System.Threading.Tasks;
using Code.Connection.Code.Connection;
using Code.Infrastructure.States.StateMachine;
using Code.Online.Data;
using Photon.Client;
using Photon.Realtime;
using UnityEngine;

namespace Code.Connection
{
    public class Matchmaker
    {
        public event Action<string> OnMatchmakingFailed;
        public event Action OnMatchmakingSuccess;
        
        private readonly PhotonClientArgsProvider _photonClientArgsProvider;
        private readonly IGameStateMachine _gameStateMachine;
        private readonly RoomWatcher _roomWatcher;
        
        private RealtimeClient _client;
        private AppSettings _appSettings;
        private CancellationTokenSource _cancellation;

        private int _maxPlayers;
        private int _playersForMatchmaking => _photonClientArgsProvider.RuntimeLocalPlayer.createDummy ? 1 : 2;

        public Matchmaker(
            PhotonClientArgsProvider photonClientArgsProvider,
            IGameStateMachine gameStateMachine,
            RoomWatcher roomWatcher)
        {
            _photonClientArgsProvider = photonClientArgsProvider;
            _gameStateMachine = gameStateMachine;
            _roomWatcher = roomWatcher;
        }
        
        public async void StartMatchmaking()
        {
            _client = _photonClientArgsProvider.Client;
            if (_client == null || !_client.IsConnected)
            {
                Debug.LogError("[Matchmaker] Client is null or not connected to Master.");
                OnMatchmakingFailed?.Invoke("Not connected to Photon.");
                return;
            }
            
            _cancellation = new CancellationTokenSource();
            Debug.Log("[Matchmaker] Starting matchmaking...");

            try
            {
                var asyncConfig = new AsyncConfig
                {
                    CancellationToken = _cancellation.Token,
                    TaskFactory = AsyncConfig.CreateUnityTaskFactory()
                };

                var arguments = new MatchmakingArguments
                {
                    PhotonSettings = new AppSettings(_appSettings),
                    PluginName = "QuantumPlugin",
                    MaxPlayers = _playersForMatchmaking,
                    AsyncConfig = asyncConfig,
                    NetworkClient = _client,
                };

                _maxPlayers = arguments.MaxPlayers;

                short result = await _client.JoinRandomOrCreateRoomAsync(
                    BuildJoinRandomRoomArgs(arguments),
                    BuildEnterRoomArgs(arguments),
                    config: asyncConfig);

                if (result == ErrorCode.Ok)
                {
                    Debug.Log($"[Matchmaker] Joined room: {_client.CurrentRoom.Name}");

                    _roomWatcher.Initialize(_client, _maxPlayers);
                    
                    _client.LocalPlayer.SetCustomProperties(new PhotonHashtable
                    {
                        { nameof(MatchPlayerDataType.Nickname), $"{_photonClientArgsProvider.ConnectArgs.RuntimePlayers[0].PlayerNickname}" },
                        { nameof(MatchPlayerDataType.UserId), $"{_client.LocalPlayer.UserId}" },
                    });

                    OnMatchmakingSuccess?.Invoke();
                }
                else
                {
                    Debug.LogError($"[Matchmaker] Failed with error code: {result}");
                    OnMatchmakingFailed?.Invoke($"Error code: {result}");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"[Matchmaker] Matchmaking exception: {ex.Message}");
                OnMatchmakingFailed?.Invoke(ex.Message);
            }
        }
        
        public async Task LeaveRoomAsync()
        {
            if (_client == null)
            {
                Debug.LogWarning("[Matchmaker] LeaveRoom called, but client is null.");
                return;
            }

            if (_client.CurrentRoom == null)
            {
                Debug.LogWarning("[Matchmaker] LeaveRoom called, but no room joined.");
                return;
            }

            try
            {
                Debug.Log($"[Matchmaker] Leaving room: {_client.CurrentRoom.Name}...");
                await _client.LeaveRoomAsync();
                Debug.Log("[Matchmaker] Successfully left the room.");
            }
            catch (Exception e)
            {
                Debug.LogError($"[Matchmaker] Exception while leaving room: {e.Message}");
            }

            _roomWatcher.Cleanup();
        }


        private void OnDestroy()
        {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
        }

        private JoinRandomRoomArgs BuildJoinRandomRoomArgs(MatchmakingArguments arguments)
        {
            return new JoinRandomRoomArgs
            {
                Ticket = arguments.Ticket,
                ExpectedUsers = arguments.ExpectedUsers,
                ExpectedCustomRoomProperties = arguments.CustomProperties,
                SqlLobbyFilter = arguments.SqlLobbyFilter,
                ExpectedMaxPlayers = arguments.MaxPlayers,
                Lobby = arguments.Lobby,
                MatchingType = arguments.RandomMatchingType
            };
        }

        private EnterRoomArgs BuildEnterRoomArgs(MatchmakingArguments arguments)
        {
            return new EnterRoomArgs
            {
                RoomName = arguments.RoomName,
                Lobby = arguments.Lobby,
                Ticket = arguments.Ticket,
                ExpectedUsers = arguments.ExpectedUsers,
                RoomOptions = new RoomOptions()
                {
                    MaxPlayers = (byte)arguments.MaxPlayers,
                    IsOpen = true,
                    IsVisible = true,
                    DeleteNullProperties = true,
                    PlayerTtl = arguments.PlayerTtlInSeconds * 1000,
                    EmptyRoomTtl = arguments.EmptyRoomTtlInSeconds * 1000,
                    Plugins = arguments.Plugins,
                    SuppressRoomEvents = false,
                    SuppressPlayerInfo = false,
                    PublishUserId = true,
                    CustomRoomProperties = arguments.CustomProperties,
                    CustomRoomPropertiesForLobby = arguments.CustomLobbyProperties,
                }
            };
        }
    }
}
