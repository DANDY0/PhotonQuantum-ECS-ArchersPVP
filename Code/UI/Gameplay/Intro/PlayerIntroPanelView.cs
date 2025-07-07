using Code.Online.Data;
using TMPro;
using UnityEngine;

namespace Code.UI.Gameplay.Intro
{
    namespace Code.UI.Gameplay.Intro
    {
        public class PlayerIntroPanelView : MonoBehaviour
        {
            [SerializeField] private TextMeshProUGUI _nicknameText;

            public void SetData(MatchPlayerData data)
            {
                _nicknameText.text = data.Nickname;
                // TODO: set avatar or other data
            }
        }
    }
}