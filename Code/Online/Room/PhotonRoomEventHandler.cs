using System;
using System.Collections.Generic;
using Code.Connection;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Online.Data;
using Code.UI.Windows;
using Code.Utils;
using Photon.Client;
using Photon.Realtime;
using UnityEngine;
using Zenject;

public enum PhotonEventCode : byte
{
    PlayerReady = 1,
    StartGame = 2,
    MatchIntroData
}

public class PhotonRoomEventHandler : IInRoomCallbacks, IOnEventCallback, IInitializable, IDisposable
{
    private readonly IGameStateMachine _gameStateMachine;
    private readonly MatchSyncDataService _matchIntroDataService;
    private readonly PhotonClientArgsProvider _photonClientArgsProvider;
    public event Action AllPlayersReadyEvent;
    
    private RealtimeClient _client;
    private HashSet<string> _readyPlayers = new HashSet<string>();
    
    private const string IsReadyKey = "IsReady";

    public PhotonRoomEventHandler(
        IGameStateMachine gameStateMachine,
        MatchSyncDataService matchIntroDataService,
        PhotonClientArgsProvider photonClientArgsProvider)
    {
        _gameStateMachine = gameStateMachine;
        _matchIntroDataService = matchIntroDataService;
        _photonClientArgsProvider = photonClientArgsProvider;
    }

    public void Initialize()
    {
        _photonClientArgsProvider.RealtimeClientSetEvent += RealtimeClientSetHandler;
    }

    public void Dispose()
    {
        _photonClientArgsProvider.RealtimeClientSetEvent -= RealtimeClientSetHandler;
    }

    private void RealtimeClientSetHandler(RealtimeClient obj)
    {
        _client = _photonClientArgsProvider.Client ?? throw new ArgumentNullException(nameof(_photonClientArgsProvider.Client));
        _client.AddCallbackTarget(this);
    }

    public void OnEvent(EventData photonEvent)
    {
        switch (photonEvent.Code)
        {
            case (byte)PhotonEventCode.MatchIntroData:
            {
                var raw = photonEvent.CustomData as object[];
                if (raw == null)
                {
                    Debug.LogError("MatchIntroData event received, but data was not object[].");
                    return;
                }

                var data = MatchSyncData.FromPhoton(raw);
                _matchIntroDataService.Set(data);
                SendPlayerReady();
                break;
            }


            case (byte)PhotonEventCode.PlayerReady:
                HandlePlayerReady(photonEvent);
                break;
            
            case (byte)PhotonEventCode.StartGame:
                Debug.Log("Received StartGame event");
                _gameStateMachine.Enter<BattleState>();
                break;
        }
    }

    private void SendPlayerReady()
    {
        _client = _photonClientArgsProvider.Client ?? throw new ArgumentNullException(nameof(_photonClientArgsProvider.Client));
        var playerId = _client.LocalPlayer.UserId;

        if (!_client.OpRaiseEvent(
            (byte)PhotonEventCode.PlayerReady,
            playerId,
            new RaiseEventArgs() { Receivers = ReceiverGroup.MasterClient },
            SendOptions.SendReliable))
        {
            Debug.LogError("Failed to send PlayerReady event");
        }
        else
        {
            Debug.Log("PlayerReady event sent");
        }
    }
    
    public void SendMatchIntroData(MatchSyncData data)
    {
        var success = _client.OpRaiseEvent(
            (byte)PhotonEventCode.MatchIntroData,
            data.ToPhoton(),
            new RaiseEventArgs { Receivers = ReceiverGroup.All },
            SendOptions.SendReliable
        );

        if (success)
            Debug.Log("MatchIntroData event sent");
        else
            Debug.LogError("Failed to send MatchIntroData event");
    }
    
    private void SendStartGameEvent()
    {
        if (!_client.OpRaiseEvent(
            (byte)PhotonEventCode.StartGame,
            null,
            new RaiseEventArgs() { Receivers = ReceiverGroup.All },
            SendOptions.SendReliable))
        {
            Debug.LogError("Failed to send StartGame event");
        }
        else
        {
            Debug.Log("StartGame event sent");
        }
    }

    public void OnPlayerPropertiesUpdate(Player targetPlayer, PhotonHashtable changedProps)
    {
        if (changedProps.ContainsKey(IsReadyKey)) 
            RestoreReadyState();
    }

    public void OnMasterClientSwitched(Player newMasterClient)
    {
        Debug.Log($"Master client switched to {newMasterClient.UserId}");

        if (_client.LocalPlayer.IsMasterClient)
        {
            Debug.Log("I am the new master client");
            RestoreReadyState();
        }
    }

    private void RestoreReadyState()
    {
        _readyPlayers.Clear();

        foreach (var player in _client.CurrentRoom.Players.Values)
        {
            if (player.CustomProperties.TryGetValue(IsReadyKey, out var isReady) && (bool)isReady)
            {
                _readyPlayers.Add(player.UserId);
            }
        }

        Debug.Log($"Restored ready players count: {_readyPlayers.Count}/{_client.CurrentRoom.PlayerCount}");

        if (_readyPlayers.Count >= _client.CurrentRoom.PlayerCount)
        {
            SendStartGameEvent();
            AllPlayersReadyEvent?.Invoke();
        }
    }

    public void OnPlayerEnteredRoom(Player newPlayer) { }

    public void OnPlayerLeftRoom(Player otherPlayer) { }

    public void OnRoomPropertiesUpdate(PhotonHashtable propertiesThatChanged) { }

    private void HandlePlayerReady(EventData photonEvent)
    {
        if (photonEvent.CustomData is string playerId)
        {
            if (_readyPlayers.Add(playerId))
            {
                Debug.Log($"Player {playerId} is ready. Total ready: {_readyPlayers.Count}/{_client.CurrentRoom.PlayerCount}");

                if (_readyPlayers.Count >= _client.CurrentRoom.PlayerCount)
                {
                    if (_client.LocalPlayer.IsMasterClient)
                    {
                        Debug.Log("All players ready, master starting game...");
                        SendStartGameEvent();
                        AllPlayersReadyEvent?.Invoke();
                    }
                }
            }
        }
        else
        {
            Debug.LogWarning("PlayerReady event received with invalid data");
        }
    }
}
