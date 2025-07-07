using System;
using Quantum;
using SimpleInputNamespace;
using UnityEngine;

namespace Code.UI.Gameplay
{
    public unsafe class JoysticksHolderView : MonoBehaviour
    {
        [SerializeField] CanvasGroup _mainCanvasGroup;
        [SerializeField] Joystick _movementsJoystick;
        [SerializeField] Joystick _ultimateJoystick;
        
        private void Awake()
        {
            SetEnableJoysticksState(false);
            
            QuantumEvent.Subscribe<EventOnGameRunning>(this, OnGameRunning);
            QuantumEvent.Subscribe<EventOnRoundStartCountDown>(this, OnRoundStartCountDown);
            
            CheckIfGameAlreadyRunning();
        }


        //for reconnection or weak connection cases

        private void CheckIfGameAlreadyRunning()
        {
            Frame frame = QuantumRunner.Default?.Game?.Frames.Verified;
            if (frame != null)
            {
                if (frame.Global->GameState != GameState.Initialization && 
                    frame.Global->GameState != GameState.MatchIntro && 
                    frame.Global->GameState != GameState.RoundStartCountDown)
                    OnGameRunning(null);
            }
        }

        private void OnGameRunning(EventOnGameRunning callback)
        {
            SetEnableJoysticksState(true);
        }
        
        private void OnRoundStartCountDown(EventOnRoundStartCountDown callback)
        {
            SetEnableJoysticksState(false);
        }

        private void SetEnableJoysticksState(bool enable)
        {
            _mainCanvasGroup.alpha = enable ? 1 : 0;
            _movementsJoystick.enabled = enable;
            _ultimateJoystick.enabled = enable;
        }
    }
}