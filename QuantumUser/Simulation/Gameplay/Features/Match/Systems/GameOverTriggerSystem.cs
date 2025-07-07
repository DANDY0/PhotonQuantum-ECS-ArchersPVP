using Quantum.Assets;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.Gameplay.Features.Match
{
    [Preserve]
    public unsafe class GameOverTriggerSystem : SystemSignalsOnly, ISignalOnRoundEnded
    {
        public void OnRoundEnded(Frame f, Owner winner, Owner loser, QBoolean isDraw)
        {
            if (isDraw)
                return;

            int winnerTeamIndex = winner.TeamIndex;
            int winnerScore = f.Global->MatchScore[winnerTeamIndex];
            GameSettingsData gameSettingsData = f.FindAsset<GameSettingsData>(f.RuntimeConfig.GameSettingsData.Id);
            
            if (winnerScore >= (gameSettingsData.BestOfCountRounds + 1) / 2)
            {
                Debug.Log($"[GameOverTriggerSystem] Team {winnerTeamIndex} won. GAME OVER!");
                f.Signals.OnGameOver(winner);
            }
            else
                Debug.Log($"[GameOverTriggerSystem] Round Ended, game continues");
        }
    }
}