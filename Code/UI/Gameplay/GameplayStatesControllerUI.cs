using Code.Connection;
using Code.Player.Camera;
using Code.UI.Gameplay.Intro;
using Code.UI.Gameplay.Windows.GameOver;
using Code.UI.Gameplay.Windows.MatchScore;
using Code.UI.Windows;
using Photon.Realtime;
using Quantum;
using Quantum.Assets;
using UnityEngine;
using Zenject;

namespace Code.UI.Gameplay
{
    public unsafe class GameplayStatesControllerUI : QuantumSceneViewComponent<ViewsContext>, IInitializable, IGameplayStatesControllerUI 
    {
        private IWindowService _windowService;
        private GameSettingsData _gameSettingsData;
        private IMatchIntroWindowController _matchIntroWindowController;
        private ICountdownWindowController _countdownWindowController;
        private IMatchScoreWindowController _matchScoreWindowController;
        private IGameOverWindowController _gameOverWindowController;
        private PhotonClientArgsProvider _photonClientArgsProvider;

        [Inject]
        private void Construct(
            IMatchIntroWindowController matchIntroWindowController,
            ICountdownWindowController countdownWindowController,
            IMatchScoreWindowController matchScoreWindowController,
            IGameOverWindowController gameOverWindowController,
            PhotonClientArgsProvider photonClientArgsProvider,
            IWindowService windowService)
        {
            _countdownWindowController = countdownWindowController;
            _matchIntroWindowController = matchIntroWindowController;
            _matchScoreWindowController = matchScoreWindowController;
            _gameOverWindowController = gameOverWindowController;
            _photonClientArgsProvider = photonClientArgsProvider;
            _windowService = windowService;
        }

        private void Awake()
        {
            QuantumEvent.Subscribe<EventOnMatchIntro>(this, OnMatchIntro);
            QuantumEvent.Subscribe<EventOnRoundStartCountDown>(this, OnRoundStartCountDown);
            QuantumEvent.Subscribe<EventOnGameRunning>(this, OnGameRunning);
            QuantumEvent.Subscribe<EventOnRoundEnded>(this, OnRoundEnded);
            QuantumEvent.Subscribe<EventOnGameOver>(this, OnGameOver);
            QuantumEvent.Subscribe<EventOnGameRestarted>(this, OnGameRestarted);
        }

        public override void OnActivate(Frame frame)
        {
            if (frame != null)
            {
                _gameSettingsData = frame.FindAsset<GameSettingsData>(frame.RuntimeConfig.GameSettingsData.Id);

                //TODO INIT SCORES
                // _blueTeamScoreText.text = frame.Global->TeamScore[0].ToString();
                // _redTeamScoreText.text = frame.Global->TeamScore[1].ToString();

                SetGameTimerText(_gameSettingsData.GameDuration.AsFloat);
                SetMatchScore();

                if (frame.Global->GameState != GameState.Initialization && 
                    frame.Global->GameState != GameState.MatchIntro &&
                    frame.Global->GameState != GameState.RoundStartCountDown)
                    OnGameRunning(null);
            }
        }

        private void SetMatchScore()
        {
            
        }

        public void Initialize()
        {
            Debug.Log("GameplayControllerUI INITIALIZED");
        }

        private void LateUpdate()
        {
            Frame frame = QuantumRunner.Default?.Game?.Frames.Verified;
            if (frame != null && frame.Global->GameState == GameState.Running)
            {
                float gameTimeLeft = frame.Global->MainGameTimer.TimeLeft.AsFloat;
                SetGameTimerText(gameTimeLeft);
            }
        }

        private void OnMatchIntro(EventOnMatchIntro callback)
        {
            Frame frame = QuantumRunner.Default?.Game?.Frames.Predicted;

            if (frame != null)
            {
                if(_gameSettingsData == null)
                        _gameSettingsData = frame.FindAsset<GameSettingsData>(frame.RuntimeConfig.GameSettingsData.Id);

                Debug.Log("GameplayControllerUI OnMatchIntro STATE");
            
                _windowService.Close(WindowId.LoadingMatchmaking);
                _windowService.Close(WindowId.Menu);
            
                _windowService.Open(WindowId.MatchIntro);
                _matchIntroWindowController?.StartMatchIntro(_gameSettingsData.MatchIntroDuration.AsFloat); 
            }
            else
            {
                Debug.Log("GameplayControllerUI OnMatchIntro FRAME IS NULL ERROR ERROR");
            }
      
        }

        private void OnRoundStartCountDown(EventOnRoundStartCountDown callback)
        {
            _windowService.Open(WindowId.CountDown);
            _countdownWindowController.StartCountdown(_gameSettingsData.CountDownDuration.AsFloat);

            if (callback.IsFirst)
                _windowService.Open(WindowId.MatchScore);

            Frame frame = QuantumRunner.Default?.Game?.Frames.Verified;

            string leftNickname = "YOU";
            string enemyNickname = "BOT";

            try
            {
                var ourLink = ViewContext.OurPlayerOwner.Link.Value;
                var enemyLink = ViewContext.EnemyPlayerOwner.Link.Value;

                var ourData = frame.GetPlayerData(ourLink);
                var enemyData = frame.GetPlayerData(enemyLink);

                leftNickname = ourData.PlayerNickname;

                enemyNickname = _photonClientArgsProvider.RuntimeLocalPlayer.createDummy
                    ? "BOT" 
                    : enemyData.PlayerNickname;
            }
            catch (MatchmakingExtensions.Exception e)
            {
                Debug.LogError($"[OnRoundStartCountDown] Failed to get player nicknames: {e.Message}");
            }

            _matchScoreWindowController.InitView(leftNickname, enemyNickname, _gameSettingsData.BestOfCountRounds);

            Debug.Log($"[OnRoundStartCountDown] Left: {leftNickname}, Enemy: {enemyNickname}");

            if (callback.IsFirst)
            {
                // PlaySound(_gameStartingFirstSound);
            }
            else
            {
                // PlaySound(_gameStartingNormalSound);
            }
        }


        private void OnGameRunning(EventOnGameRunning eventData)
        {
            Debug.Log("GameplayControllerUI OnGameRunning STATE");
            
            //TODO
            //Sd
        }

        private void OnRoundEnded(EventOnRoundEnded eventData)
        {
            Frame frame = eventData.Game.Frames.Verified;

            int ourTeamIndex = ViewContext.OurPlayerOwner.TeamIndex;
            int enemyTeamIndex = ViewContext.EnemyPlayerOwner.TeamIndex;

            _matchScoreWindowController.SetMatchScore(frame.Global->MatchScore, ourTeamIndex, enemyTeamIndex);
        }

        private void OnGameOver(EventOnGameOver eventData)
        {
            Frame frame = eventData.Game.Frames.Verified;
            _windowService.Open(WindowId.GameOver);
            string winnerNickname = frame.GetPlayerData(eventData.winner.Link.Value).PlayerNickname;
            _gameOverWindowController.SetWinner(winnerNickname);
            //TODO game over effects
        }

        private void OnGameRestarted(EventOnGameRestarted eventData)
        {
        }

        private void SetGameTimerText(float gameTimeLeft)
        {
//            Debug.Log($"gameTimeeft: {gameTimeLeft}");
            //TODO set timer view data
            /*int minutes = Mathf.FloorToInt(gameTimeLeft / 60f);
            int seconds = Mathf.FloorToInt(gameTimeLeft % 60f);

            _gameTimerText.text = $"{minutes}:{seconds:00}";*/
        }
    }
}