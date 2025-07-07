using UnityEngine;

public class PlayerHiddenDebugGizmo : MonoBehaviour
{
	public float radius = 0.5f;
	public Color gizmoColor = new Color(0f, 1f, 0f, 0.4f);

	private void OnDrawGizmosSelected()
	{
		Gizmos.color = gizmoColor;
		Gizmos.DrawSphere(transform.position, radius);
	}
}
