using Code.Player.Camera;
using Quantum;
using UnityEngine;

namespace Code.Player
{
	public class PlayerContextSetter: QuantumEntityViewComponent<ViewsContext>
	{
		public override void OnActivate(Frame frame)
		{
			base.OnActivate(frame);
			
			PlayerLink playerLink = VerifiedFrame.Get<PlayerLink>(EntityRef);
			bool isDummy = VerifiedFrame.Has<Dummy>(EntityRef);

			if(isDummy)
				return;

			if (Game.PlayerIsLocal(playerLink.Value))
			{
				ViewContext.OurPlayerGameObject = gameObject;
				ViewContext.OurPlayerOwner = frame.Get<Owner>(playerLink.Entity);
			}
			else
			{
				ViewContext.EnemyPlayerGameObject = gameObject;
				ViewContext.EnemyPlayerOwner = frame.Get<Owner>(playerLink.Entity);
			}
		}
	}
}