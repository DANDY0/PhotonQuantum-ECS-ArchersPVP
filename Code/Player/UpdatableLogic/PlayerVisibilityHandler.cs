using Quantum;
using UnityEngine;

namespace Code.Player
{
	public class PlayerVisibilityHandler: QuantumEntityViewComponent
	{
		[SerializeField] private PlayerUIHandler _playerUIHandler;
		[SerializeField] private PlayerAttackRangeView _playerAttackRangeView;

		[SerializeField] private SkinnedMeshRenderer _headRenderer;
		[SerializeField] private SkinnedMeshRenderer _bodyRenderer;
		[SerializeField] private SkinnedMeshRenderer _bowRenderer;
	
		private bool _isLocalPLayer;
		
		public override void OnActivate(Frame frame)
		{
			_isLocalPLayer = Game.PlayerIsLocal(VerifiedFrame.Get<PlayerLink>(_entityView.EntityRef).Value);
		}

		public override void OnUpdateView()
		{
			if (_isLocalPLayer)
				return;
			
			bool isVisible = VerifiedFrame.Has<IsVisibleToEnemies>(_entityView.EntityRef);
			
			_headRenderer.enabled = isVisible;
			_bodyRenderer.enabled = isVisible;
			_bowRenderer.enabled = isVisible;
			_playerUIHandler.SetVisibility(isVisible);
			_playerAttackRangeView.SetVisibility(isVisible);
		}
	}
}