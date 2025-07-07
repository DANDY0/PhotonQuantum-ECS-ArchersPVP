using UnityEngine;
using UnityEngine.UI;

namespace Code.Input.Joystick
{
    public class UltimateJoystick : JoystickOnScene
    {
        [SerializeField] private Image[] parts;
        
        public override void SetInteractable(bool interactable)
        {
            foreach (var part in parts)
                part.raycastTarget = interactable;
        }
    }
}