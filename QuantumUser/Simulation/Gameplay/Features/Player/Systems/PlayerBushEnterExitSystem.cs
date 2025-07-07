using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Bushes.Systems
{
	[Preserve]
	public unsafe class PlayerBushEnterExitSystem : SystemMainThreadFilter<PlayerBushEnterExitSystem.Filter>
	{
		private const int BushLayerMask = 1 << 8; //TODO use layer

		public override void Update(Frame f, ref Filter filter)
		{
			if (filter.PlayerLifeState->Value == EPlayerLifeState.Dead)
				return;

			var position = filter.Transform3D->Position;

			//TODO add radius to some SO

			var hits = f.Physics3D.OverlapShape(
				position,
				FPQuaternion.Identity,
				Shape3D.CreateSphere(FP._0_50)
				// layerMask: BushLayerMask
			);

			bool isInBush = false;
			for (int i = 0; i < hits.Count; i++)
			{
				if (f.Has<BushCollider>(hits[i].Entity))
				{
					isInBush = true;
					break;
				}
			}

			bool currentlyInBush = f.Has<InBush>(filter.Entity);

			if (isInBush && !currentlyInBush)
				f.Add<InBush>(filter.Entity);

			if (!isInBush && currentlyInBush)
				f.Remove<InBush>(filter.Entity);
		}

		public struct Filter
		{
			public EntityRef Entity;
			public Transform3D* Transform3D;
			public PlayerLifeState* PlayerLifeState;
		}
	}
}