using Code.UI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI.Gameplay.Windows.MatchScore
{
    public class MatchScoreWindow : BaseWindow
    {
        public override WindowId Id => WindowId.MatchScore;

        [SerializeField] private TextMeshProUGUI _leftScoreValue;
        [SerializeField] private TextMeshProUGUI _rightScoreValue;
        [SerializeField] private Image _leftScoreImage;
        [SerializeField] private Image _rightScoreImage;

        [SerializeField] private TextMeshProUGUI _leftNicknameText;
        [SerializeField] private TextMeshProUGUI _rightNicknameText;
        [SerializeField] private TextMeshProUGUI _bestOfText;
        
        [SerializeField] private Color _ourColor;
        [SerializeField] private Color _enemyColor;

        public void InitView(string ourNickname, string enemyNickname, int bestOfValue)
        {
            _leftNicknameText.text = ourNickname;
            _rightNicknameText.text = enemyNickname;

            _leftScoreImage.color = _ourColor;
            _rightScoreImage.color = _enemyColor;

            _bestOfText.text = $"Bo{bestOfValue}";
        }

        public void SetMatchScore(ScoreElementData leftScoreElement, ScoreElementData rightScoreElement)
        {
            _leftScoreValue.text = leftScoreElement.ScoreValue.ToString();
            _rightScoreValue.text = rightScoreElement.ScoreValue.ToString();
        }
    }

    public struct ScoreElementData
    {
        public bool IsOur;
        public int ScoreValue;
    }
}