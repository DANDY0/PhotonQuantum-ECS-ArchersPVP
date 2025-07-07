using Quantum;

namespace Code.UI.Gameplay.Windows.MatchScore
{
    public interface IMatchScoreWindowController
    {
        public unsafe void SetMatchScore(int* matchScore, int ourTeamIndex, int enemyTeamIndex);
        public void InitView(string ourNickname, string enemyNickname, int bestOfValue);
    }
}