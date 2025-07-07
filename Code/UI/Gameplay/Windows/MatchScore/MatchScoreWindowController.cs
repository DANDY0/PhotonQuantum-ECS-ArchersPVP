using Code.UI.Gameplay.Intro;
using Code.UI.Windows;
using Quantum;

namespace Code.UI.Gameplay.Windows.MatchScore
{
    public class MatchScoreWindowController : BaseController<MatchScoreWindow>, IMatchScoreWindowController
    {
        public void InitView(string ourNickname, string enemyNickname, int bestOfValue)
        {
            View.InitView(ourNickname, enemyNickname, bestOfValue);
        }

        public unsafe void SetMatchScore(int* matchScore, int ourTeamIndex, int enemyTeamIndex)
        {
            var leftScoreData = new ScoreElementData
            {
                IsOur = true,
                ScoreValue = matchScore[ourTeamIndex]
            };

            var rightScoreData = new ScoreElementData
            {
                IsOur = false,
                ScoreValue = matchScore[enemyTeamIndex]
            };

            View.SetMatchScore(leftScoreData, rightScoreData);
        }

    }
}