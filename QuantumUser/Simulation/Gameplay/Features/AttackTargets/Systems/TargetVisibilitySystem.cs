using ModestTree.Util;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets.Systems
{
	[Preserve]
	public unsafe class TargetVisibilitySystem : SystemMainThreadFilter<TargetVisibilitySystem.Filter>
	{
		public override void Update(Frame f, ref Filter filter)
		{
			if (filter.PlayerLifeState->Value == EPlayerLifeState.Dead)
				return;
        
			var from = filter.Transform3D->Position;
			var to = f.Get<Transform3D>(filter.Target->Value.Entity).Position;

			bool obstacleBetweenTarget = HasObstacle(f, from, to);
            
			if (obstacleBetweenTarget)
				f.Add<ObstaclePreventsAttack>(filter.Entity);
			else
			{
				if (f.Has<ObstaclePreventsAttack>(filter.Entity))
					f.Remove<ObstaclePreventsAttack>(filter.Entity);
			}
		}

		private bool HasObstacle(Frame frame, FPVector3 source, FPVector3 target)
		{
			//TODO to config
			Physics3D.HitCollection3D hits = frame.Physics3D.LinecastAll(source, target, frame.Layers.GetLayerMask("Obstacle"));
			for (int i = 0; i < hits.Count; i++)
			{
				Debug.Log($"Has obstacle between");
				return true;
			}
            
			Debug.Log($"No obstacle between");
			return false;
		}

		public struct Filter
		{
			public EntityRef Entity;
			public PlayerRef* Ref;
			public PlayerLink* Link;
			public CurrentTarget* Target;
			public Transform3D* Transform3D;
			public PlayerLifeState* PlayerLifeState;
		}
	}
}