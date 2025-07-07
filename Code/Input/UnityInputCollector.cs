using Code.Input.Joystick;
using Photon.Deterministic;
using Quantum;
using UnityEngine;

namespace Code.Input
{
    public class UnityInputCollector : MonoBehaviour
    {
        [SerializeField] private JoystickOnScene _ultimateJoystick;
        [SerializeField] private JoystickOnScene _movementJoystick;

        private bool isUlting;
        private bool isMoving;

        private FPVector3 _previousUltDirection;

        private void OnEnable() => 
            QuantumCallback.Subscribe(this, (CallbackPollInput callback) => PollInput(callback));

        private void PollInput(CallbackPollInput callback)
        {
            Quantum.Input i = new Quantum.Input();
            i.LeftButton = UnityEngine.Input.GetKey(KeyCode.A) || UnityEngine.Input.GetKey(KeyCode.LeftArrow);
            i.RightButton = UnityEngine.Input.GetKey(KeyCode.D) || UnityEngine.Input.GetKey(KeyCode.RightArrow);
            i.UpButton = UnityEngine.Input.GetKey(KeyCode.W) || UnityEngine.Input.GetKey(KeyCode.UpArrow);
            i.DownButton = UnityEngine.Input.GetKey(KeyCode.S) || UnityEngine.Input.GetKey(KeyCode.DownArrow);

            Vector3 direction = new Vector3
            {
                x = SimpleInput.GetAxisRaw("Horizontal2"),
                z = SimpleInput.GetAxisRaw("Vertical2")
            };

            Vector3 ultDirection = new Vector3
            {
                x = SimpleInput.GetAxisRaw("Horizontal3"),
                z = SimpleInput.GetAxisRaw("Vertical3")
            };

            if (direction != Vector3.zero)
            {
                if (!isMoving)
                {
                    isMoving = true;
                    isUlting = false;
                    _ultimateJoystick.SetInteractable(false);
                    _movementJoystick.SetInteractable(true);
                }
            }
            else if (ultDirection != Vector3.zero)
            {
                if (!isUlting)
                {
                    isUlting = true;
                    isMoving = false;
                    _movementJoystick.SetInteractable(false);
                    _ultimateJoystick.SetInteractable(true);
                }
            }

            if (isUlting && ultDirection == Vector3.zero && _previousUltDirection != FPVector3.Zero)
            {
                
                isUlting = false;
                i.IsUltimate = true;
                i.FinalUltDirection = _previousUltDirection;
                
                _movementJoystick.SetInteractable(true);
            }


            i.Direction = direction.ToFPVector3();
            i.UltDirection = ultDirection.ToFPVector3();
            _previousUltDirection = ultDirection.ToFPVector3();
            
            if (direction == Vector3.zero && ultDirection == Vector3.zero)
            {
                _ultimateJoystick.SetInteractable(true);
                _movementJoystick.SetInteractable(true);
                isMoving = false;
                isUlting = false;
            }

            callback.SetInput(i, DeterministicInputFlags.Repeatable);
        }
    }
}
