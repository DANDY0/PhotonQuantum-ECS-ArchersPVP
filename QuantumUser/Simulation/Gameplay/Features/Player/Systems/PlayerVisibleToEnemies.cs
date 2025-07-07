using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Bushes.Systems
{
	[Preserve]
	public unsafe class PlayerVisibleToEnemies : SystemMainThreadFilter<PlayerVisibleToEnemies.Filter>
	{
		public override void Update(Frame f, ref Filter filter)
		{
			bool isInBush = f.Has<InBush>(filter.Entity);
			bool isRevealed = f.Has<BushTemporaryReveal>(filter.Entity);
			bool isEnemyNearby = IsEnemyNearby(f, filter);

			bool shouldBeVisible = !isInBush || isRevealed || isEnemyNearby;
			bool currentlyVisible = f.Has<IsVisibleToEnemies>(filter.Entity);

			if (shouldBeVisible && !currentlyVisible)
				f.Add<IsVisibleToEnemies>(filter.Entity);
			else if (!shouldBeVisible && currentlyVisible)
				f.Remove<IsVisibleToEnemies>(filter.Entity);
		}

		private bool IsEnemyNearby(Frame f, Filter filter)
		{
			var position = filter.Transform3D->Position;

			//TODO add radius to some SO
			var hits = f.Physics3D.OverlapShape(
				position,
				FPQuaternion.Identity,
				Shape3D.CreateSphere(FP._1_50)
			);

			for (int i = 0; i < hits.Count; i++)
			{
				var hitEntity = hits[i].Entity;

				// skip ourselves
				if (hitEntity == filter.Entity)
					continue;

				if (!f.TryGet<Owner>(hitEntity, out var owner))
					continue;

				//skip allies
				if (owner.TeamIndex == filter.Owner->TeamIndex)
					continue;
				
				// skip dead players
				if (f.TryGet<PlayerLifeState>(hitEntity, out var lifeState) &&
				    lifeState.Value == EPlayerLifeState.Dead)
					continue;

				return true; // found enemy in radius
			}

			return false;
		}

		public struct Filter
		{
			public EntityRef Entity;
			public Transform3D* Transform3D;
			public PlayerLink* PlayerLink;
			public Owner* Owner;
			public PlayerLifeState* PlayerLifeState;
		}
	}
}