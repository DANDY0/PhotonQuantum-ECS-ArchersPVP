using UnityEngine;
using System;
using System.Threading.Tasks;
using Code.UI.Windows;

namespace Code.UI.Gameplay.Intro
{
    public class CountdownWindowController : BaseController<CountdownWindow>, ICountdownWindowController
    {
        private readonly IWindowService _windowService;
        
        private float _countdownTime;
        private bool _isCountingDown = false;

        public CountdownWindowController(
            IWindowService windowService
            )
        {
            _windowService = windowService;
        }

        public async void StartCountdown(float countdownDuration)
        {
            _countdownTime = countdownDuration;
            
            if (_isCountingDown) 
                StopCountdown();

            _isCountingDown = true;

            await CountdownRoutine();
        }

        public void StopCountdown()
        {
            _isCountingDown = false;
        }

        private async Task CountdownRoutine()
        {
            while (_countdownTime >= 0)
            {
                View.UpdateCountdownUI(_countdownTime);
                await Task.Delay(1000);
                _countdownTime--;
            }
            
            _isCountingDown = false;
            _windowService.Close(WindowId.CountDown);
        }
    }
}