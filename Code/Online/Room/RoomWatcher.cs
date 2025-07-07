using Zenject;

namespace Code.Connection
{
    using System;
    using Photon.Client;
    using Photon.Realtime;
    using UnityEngine;

    namespace Code.Connection
    {
        public class RoomWatcher : ITickable, IDisposable, IInRoomCallbacks
        {
            private RealtimeClient _client;
            private int _maxPlayers;

            public event Action OnRoomFull;
            public event Action<int> OnPlayerCountChanged;
            
            public void Initialize(RealtimeClient client, int maxPlayers)
            {
                _client = client;
                _maxPlayers = maxPlayers;

                Debug.Log($"[RoomWatcher] Initialized with maxPlayers = {_maxPlayers}");

                if (_client == null || !_client.IsConnected || _client.CurrentRoom == null)
                {
                    Debug.LogError("[RoomWatcher] Client or room not ready.");
                    return;
                }

                _client.AddCallbackTarget(this);
                CheckPlayerCount();
            }

            public void Tick()
            {
                _client?.Service();
            }

            public void OnPlayerEnteredRoom(Player newPlayer)
            {
                Debug.Log($"[RoomWatcher] Player joined: {newPlayer.UserId}");
                CheckPlayerCount();
            }

            public void OnPlayerLeftRoom(Player otherPlayer)
            {
                Debug.Log($"[RoomWatcher] Player left: {otherPlayer.UserId}");
                CheckPlayerCount();
            }

            private void CheckPlayerCount()
            {
                int count = _client.CurrentRoom?.PlayerCount ?? 0;
                Debug.Log($"[RoomWatcher] Current player count: {count}/{_maxPlayers}");

                OnPlayerCountChanged?.Invoke(count);

                if (count >= _maxPlayers)
                {
                    Debug.Log("[RoomWatcher] Room is full!");
                    OnRoomFull?.Invoke();
                    Debug.Log("[RoomWatcher] CAN LOAD GAME SCENE");
                }
            }

            public void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged)
            {
                Debug.Log($"[RoomWatcher] Room properties changed: {propertiesThatChanged}");
            }

            public void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
            {
                Debug.Log($"[RoomWatcher] Player properties changed: {changedProps}");
            }

            public void OnMasterClientSwitched(Player newMasterClient)
            {
                Debug.Log($"[RoomWatcher] MasterClient switched: {newMasterClient.UserId}");
            }

            public void Dispose()
            {
                _client?.RemoveCallbackTarget(this);
            }
            
            public void Cleanup()
            {
                if (_client != null)
                {
                    _client.RemoveCallbackTarget(this);
                    _client = null;
                }

                _maxPlayers = 0;
                OnRoomFull = null;
                OnPlayerCountChanged = null;
            }
        }
    }
}