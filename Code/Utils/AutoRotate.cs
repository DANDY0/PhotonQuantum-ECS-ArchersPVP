using UnityEngine;

public class AutoRotate : MonoBehaviour
{
    /// The rotation speed. Positive means clockwise, negative means counter clockwise.
    [SerializeField] private Vector3 RotationSpeed = Vector3.zero;

    private void Update()
    {
        Rotate();
    }

    private void Rotate()
    {
        transform.Rotate(RotationSpeed * Time.deltaTime, Space.Self);
    }
}
