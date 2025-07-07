using Quantum.Assets;
using UnityEngine;

namespace Quantum.Gameplay.Features.GameplayStates
{
    public unsafe class GameSystem : SystemMainThread, ISignalOnRoundEnded, ISignalOnGameOver
    {
        private bool _isGameOver;
        
        public override void Update(Frame frame)
        {
            switch (frame.Global->GameState)
            {
                case GameState.None:
                    UpdateGameState_None(frame);
                    break;

                case GameState.Initialization:
                    UpdateGameState_Initialization(frame);
                    break;

                case GameState.MatchIntro:
                    UpdateGameState_MatchIntro(frame);
                    break;

                case GameState.RoundStartCountDown:
                    UpdateGameState_RoundStartCountDown(frame);
                    break;

                case GameState.Running:
                    UpdateGameState_Running(frame);
                    break;

                case GameState.RoundEnded:
                    UpdateGameState_RoundEnded(frame);
                    break;

                case GameState.GameOver:
                    UpdateGameState_GameOver(frame);
                    break;
            }
        }

        private void UpdateGameState_None(Frame frame)
        {
            ChangeGameState_Initialization(frame);
        }

        private void UpdateGameState_Initialization(Frame frame)
        {
            frame.Global->GameStateTimer.Tick(frame.DeltaTime);
            if (frame.Global->GameStateTimer.IsDone)
            {
                ChangeGameState_MatchIntro(frame);
            }
        }

        private void UpdateGameState_MatchIntro(Frame frame)
        {
            frame.Global->GameStateTimer.Tick(frame.DeltaTime);
            if (frame.Global->GameStateTimer.IsDone)
            {
                ChangeGameState_RoundStartCountDown(frame, true);
            }
        }

        private void UpdateGameState_RoundStartCountDown(Frame frame)
        {
            frame.Global->GameStateTimer.Tick(frame.DeltaTime);
            if (frame.Global->GameStateTimer.IsDone)
            {
                // ToggleTeamBaseStaticColliders(frame, false);
                // frame.Signals.OnBallSpawned();

                ChangeGameState_Running(frame);
            }
        }

        private void UpdateGameState_Running(Frame frame)
        {
            /*frame.Global->MainGameTimer.Tick(frame.DeltaTime);
            if (frame.Global->MainGameTimer.IsDone)
            {
                ChangeGameState_GameOver(frame);
            }*/
        }

        public void OnRoundEnded(Frame f, Owner winner, Owner loser, QBoolean isDraw)
        {
            ChangeGameState_RoundEnded(f, winner, loser, isDraw);
        }

        public void OnGameOver(Frame f, Owner winner)
        {
            ChangeGameState_GameOver(f, winner);
        }

        private void UpdateGameState_RoundEnded(Frame frame)
        {
            frame.Global->GameStateTimer.Tick(frame.DeltaTime);
            if (frame.Global->GameStateTimer.IsDone)
            {
                RespawnPlayers(frame);
                // ToggleTeamBaseStaticColliders(frame, true);

                ChangeGameState_RoundStartCountDown(frame, false);
            }
        }

        private void UpdateGameState_GameOver(Frame frame)
        {
            frame.Global->GameStateTimer.Tick(frame.DeltaTime);
            if (frame.Global->GameStateTimer.IsDone && !_isGameOver)
            {
                
                Debug.Log("GO TO MENU, DISCONNECT");
                _isGameOver = true;
                /*RespawnPlayers(frame);

                frame.Global->MatchScore[0] = 0;
                frame.Global->MatchScore[1] = 0;

                frame.Events.OnGameRestarted();

                ChangeGameState_RoundStartCountDown(frame, true);*/
            }
        }

        private void ChangeGameState_Initialization(Frame frame)
        {
            GameSettingsData gameSettingsData =
                frame.FindAsset<GameSettingsData>(frame.RuntimeConfig.GameSettingsData.Id);

            frame.Global->GameStateTimer.Start(gameSettingsData.InitializationDuration);
            frame.Global->GameState = GameState.Initialization;

            frame.Events.OnInitialization();
        }

        private void ChangeGameState_MatchIntro(Frame frame)
        {
            GameSettingsData gameSettingsData =
                frame.FindAsset<GameSettingsData>(frame.RuntimeConfig.GameSettingsData.Id);

            frame.Global->GameStateTimer.Start(gameSettingsData.MatchIntroDuration);
            frame.Global->GameState = GameState.MatchIntro;

            frame.Events.OnMatchIntro();
        }

        private void ChangeGameState_RoundStartCountDown(Frame frame, bool isFirst)
        {
            GameSettingsData gameSettingsData =
                frame.FindAsset<GameSettingsData>(frame.RuntimeConfig.GameSettingsData.Id);

            frame.Global->GameStateTimer.Start(gameSettingsData.CountDownDuration);
            frame.Global->GameState = GameState.RoundStartCountDown;

            frame.Events.OnRoundStartCountDown(isFirst);
            frame.Signals.OnRoundStartCountDown();
        }

        private void ChangeGameState_Running(Frame frame)
        {
            GameSettingsData gameSettingsData =
                frame.FindAsset<GameSettingsData>(frame.RuntimeConfig.GameSettingsData.Id);

            /*if (frame.Global->MainGameTimer.IsDone)
            {
                frame.Global->MainGameTimer.Start(gameSettingsData.GameDuration);
            }*/

            frame.Global->GameState = GameState.Running;

            frame.Events.OnGameRunning();
            frame.Signals.OnGameRunning();
            Debug.Log($"OnGameRunning signal invoked ");
        }

        private void ChangeGameState_RoundEnded(Frame frame, Owner winner, Owner loser, bool isDraw)
        {
            GameSettingsData gameSettingsData =
                frame.FindAsset<GameSettingsData>(frame.RuntimeConfig.GameSettingsData.Id);

            frame.Global->GameStateTimer.Start(gameSettingsData.RoundEndedIntervalDuration);
            frame.Global->GameState = GameState.RoundEnded;

            frame.Events.OnRoundEnded(winner, loser, isDraw);
        }

        private void ChangeGameState_GameOver(Frame frame, Owner winner)
        {
            GameSettingsData gameSettingsData =
                frame.FindAsset<GameSettingsData>(frame.RuntimeConfig.GameSettingsData.Id);

            frame.Global->GameStateTimer.Start(gameSettingsData.GameOverDuration);
            frame.Global->GameState = GameState.GameOver;

            frame.Events.OnGameOver(winner);
        }

        private void RespawnPlayers(Frame frame)
        {
            foreach (var (playerEntityRef, link) in frame.Unsafe.GetComponentBlockIterator<PlayerLink>())
            {
                frame.Signals.RespawnPlayer(*link);
                frame.Events.PlayerRespawned(link->Entity);
            }
        }
    }
}