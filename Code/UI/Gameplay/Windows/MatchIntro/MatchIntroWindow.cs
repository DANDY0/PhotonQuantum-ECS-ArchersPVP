using System;
using System.Threading.Tasks;
using Code.Online.Data;
using Code.UI.Gameplay.Intro.Code.UI.Gameplay.Intro;
using Code.UI.Windows;
using UnityEngine;
using Zenject;

namespace Code.UI.Gameplay.Intro
{
    public class MatchIntroWindow : BaseWindow
    {
        public event Action IntroFinishedEvent;
        
        [SerializeField] private PlayersIntroView _playersIntroView;
        
        public override WindowId Id => WindowId.MatchIntro;
        
        public void StartIntro(MatchPlayerData ourData, MatchPlayerData opponentData, float introDuration)
        {
            _playersIntroView.Show(ourData, opponentData);
            _ = DelayedHideAsync(introDuration);
        }

        private async Task DelayedHideAsync(float introDuration)
        {
            await Task.Delay((int)(introDuration * 1000));

            _playersIntroView.Hide();
            IntroFinishedEvent?.Invoke();
        }
    }
}