using Code.Connection;
using Code.Infrastructure.States.StateMachine;
using Code.Online.Data;
using Code.UI.Windows;
using UnityEngine;
using Zenject;

namespace Code.UI.Gameplay.Intro
{
    public class MatchIntroWindowController : BaseController<MatchIntroWindow>, IMatchIntroWindowController
    {
        private readonly int _introDuration = 3;
        private readonly int _waitForPlayersCount = 2;

        private MatchPlayerData _ourPlayerData;
        private MatchPlayerData _opponentPlayerData;

        private PhotonRoomEventHandler _photonRoomEventHandler;
        private readonly PhotonClientArgsProvider _photonClientArgsProvider;
        private readonly MatchSyncDataService _matchIntroDataService;
        private IWindowService _windowService;
        private IGameStateMachine _gameStateMachine;
        
        private MatchIntroWindowController(
            PhotonRoomEventHandler photonRoomEventHandler,
            PhotonClientArgsProvider photonClientArgsProvider,
            MatchSyncDataService matchIntroDataService,
            IWindowService windowService,
            IGameStateMachine gameStateMachine)
        {
            _gameStateMachine = gameStateMachine;
            _windowService = windowService;
            _photonRoomEventHandler = photonRoomEventHandler;
            _photonClientArgsProvider = photonClientArgsProvider;
            _matchIntroDataService = matchIntroDataService;
        }
        
        public void StartMatchIntro(float introDuration)
        {
            var introData = _matchIntroDataService.Current;

            if (introData == null || introData.Players == null || introData.Players.Count == 0)
            {
                Debug.LogError("[MatchIntroWindowController] MatchIntroData is missing or invalid.");
                return;
            }

            string localUserId = _photonClientArgsProvider.Client.UserId;

            foreach (var player in introData.Players)
            {
                if (player.UserId == localUserId)
                {
                    _ourPlayerData = player;
                    if (_photonClientArgsProvider.RuntimeLocalPlayer.createDummy)
                        _opponentPlayerData = new MatchPlayerData
                        {
                            Nickname = "BOT",
                        };
                }
                else
                    _opponentPlayerData = player;
            }

            Debug.Log(
                $"[MatchIntro] Our player: {_ourPlayerData.Nickname}, Opponent: {_opponentPlayerData.Nickname}");
            
            View.StartIntro(_ourPlayerData, _opponentPlayerData, introDuration);
            View.IntroFinishedEvent -= IntroFinishedHandler;
            View.IntroFinishedEvent += IntroFinishedHandler;
        }

        private void IntroFinishedHandler()
        {
            _windowService.Close(WindowId.MatchIntro);
        }
    }
}