using UnityEngine;

public class BushViewHandler : MonoBehaviour
{
	[SerializeField] private Renderer[] targetRenderers;
	[SerializeField] private Material visibleMaterial;
	[SerializeField] private Material hiddenMaterial;

	private bool _isHidden = false;

	public void SetHidden(bool hidden)
	{
		if (_isHidden == hidden) return;

		_isHidden = hidden;
		var mat = hidden ? hiddenMaterial : visibleMaterial;

		foreach (var r in targetRenderers)
		{
			if (r != null)
				r.sharedMaterial = mat;
		}
	}

	public bool IsHidden => _isHidden;
}