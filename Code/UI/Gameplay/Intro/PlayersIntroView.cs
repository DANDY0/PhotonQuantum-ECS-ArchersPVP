
using Code.Online.Data;

namespace Code.UI.Gameplay.Intro
{
    using UnityEngine;

    namespace Code.UI.Gameplay.Intro
    {
        public class PlayersIntroView : MonoBehaviour
        {
            [SerializeField] private GameObject _content;
            [SerializeField] private PlayerIntroPanelView _yourPanel;
            [SerializeField] private PlayerIntroPanelView _opponentPanel;

            public void Show(MatchPlayerData yourIntroData, MatchPlayerData opponentIntroData)
            {
                _yourPanel.SetData(yourIntroData);
                _opponentPanel.SetData(opponentIntroData);
                _content.SetActive(true);
            }

            public void Hide()
            {
                _content.SetActive(false);
            }
        }
    }
    
}