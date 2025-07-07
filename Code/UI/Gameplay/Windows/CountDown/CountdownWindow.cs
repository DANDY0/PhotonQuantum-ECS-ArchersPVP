using System;
using Code.UI.Gameplay.Intro;
using Code.UI.Windows;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace Code.UI.Windows
{
    public class CountdownWindow : BaseWindow
    {
        [SerializeField] private TextMeshProUGUI _countdownText;
        
        public override WindowId Id => WindowId.CountDown;
        
        public void UpdateCountdownUI(float timeLeft)
        {
            if (timeLeft <= 0)
                _countdownText.text = "START!";
            else
                _countdownText.text = timeLeft.ToString("F0");
        }
    }
}