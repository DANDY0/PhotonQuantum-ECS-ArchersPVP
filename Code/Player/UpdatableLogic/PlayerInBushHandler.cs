using Quantum;
using UnityEngine;
using System.Collections.Generic;
using LayerMask = UnityEngine.LayerMask;

namespace Code.Player
{
	public class PlayerInBushHandler : QuantumEntityViewComponent
	{
		[SerializeField] private SkinnedMeshRenderer _headRenderer;
		[SerializeField] private SkinnedMeshRenderer _bodyRenderer;
		[SerializeField] private SkinnedMeshRenderer _bowRenderer;

		[SerializeField] private Material _visibleHeadMat;
		[SerializeField] private Material _hiddenHeadMat;
		[SerializeField] private Material _visibleBodyMat;
		[SerializeField] private Material _hiddenBodyMat;
		[SerializeField] private Material _visibleBowMat;
		[SerializeField] private Material _hiddenBowMat;

		[SerializeField] private float _bushRadius = 1.6f;

		private readonly List<BushViewHandler> _affectedBushes = new List<BushViewHandler>();

		public override void OnActivate(Frame frame)
		{
		}

		public override void OnUpdateView()
		{
			if(!Game.PlayerIsLocal(VerifiedFrame.Get<PlayerLink>(_entityView.EntityRef).Value))
				return;
			
			bool isHidden = VerifiedFrame.Has<InBush>(_entityView.EntityRef);
			
			SetAlpha(!isHidden);
			UpdateBushes(isHidden);
		}

		private void SetAlpha(bool visibility)
		{
			_headRenderer.sharedMaterial = visibility ? _visibleHeadMat : _hiddenHeadMat;
			_bodyRenderer.sharedMaterial = visibility ? _visibleBodyMat : _hiddenBodyMat;
			_bowRenderer.sharedMaterial = visibility ? _visibleBowMat : _hiddenBowMat;
		}

		private void UpdateBushes(bool makeTransparent)
		{
			foreach (var bush in _affectedBushes)
			{
				if (bush != null)
					bush.SetHidden(false);
			}
			_affectedBushes.Clear();

			if (!makeTransparent)
				return;

			int bushLayerMask = LayerMask.GetMask("Bush");
			Collider[] hits = Physics.OverlapSphere(transform.position, _bushRadius, bushLayerMask);

			foreach (var hit in hits)
			{
				var bush = hit.GetComponentInParent<BushViewHandler>();
				if (bush != null)
				{
					bush.SetHidden(true);
					_affectedBushes.Add(bush);
				}
			}
		}
		
#if UNITY_EDITOR
		private void OnDrawGizmosSelected()
		{
			Gizmos.color = new Color(0f, 1f, 0f, 0.35f);
			Gizmos.DrawSphere(transform.position, _bushRadius);
			Gizmos.color = Color.green;
			Gizmos.DrawWireSphere(transform.position, _bushRadius);
		}
#endif

	}
}
