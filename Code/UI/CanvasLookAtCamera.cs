namespace Code.UI
{
    using UnityEngine;

    public class CanvasLookAtCamera : MonoBehaviour
    {
        private Camera _mainCamera;

        void Start()
        {
            _mainCamera = Camera.main;
        }

        void LateUpdate()
        {
            if (_mainCamera == null) 
                return;

            transform.LookAt(transform.position + _mainCamera.transform.forward, _mainCamera.transform.up);
        }
    }

}