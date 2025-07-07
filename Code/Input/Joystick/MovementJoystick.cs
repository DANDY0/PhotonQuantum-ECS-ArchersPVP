using UnityEngine;

namespace Code.Input.Joystick
{
    public class MovementJoystick : JoystickOnScene
    {
        [SerializeField] private GameObject parent;
        
        public override void SetInteractable(bool interactable) => 
            parent.SetActive(interactable);
    }
}