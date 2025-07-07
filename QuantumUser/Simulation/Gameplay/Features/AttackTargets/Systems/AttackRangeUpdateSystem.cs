using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Factory;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets.Systems
{
	[Preserve]
	public unsafe class AttackRangeUpdateSystem : SystemMainThreadFilter<AttackRangeUpdateSystem.Filter>
	{
		public override void Update(Frame f, ref Filter filter)
		{
			var range = filter.AttackRange;

			bool isMoving = filter.ActionState->Value == EPlayerActionState.Moving;

			if (isMoving)
			{
				if (range->CurrentRange < range->MaxRange)
				{
					range->CurrentRange += range->ExpansionRate * f.DeltaTime;
					if (range->CurrentRange > range->MaxRange)
					{
						range->CurrentRange = range->MaxRange;
					}
				}
			}
			else
			{
				if (!f.Has<CurrentTarget>(filter.Entity))
				{
					range->CurrentRange -= range->ContractionRate * f.DeltaTime;
					if (range->CurrentRange < range->MinRange)
					{
						range->CurrentRange = range->MinRange;
					}
				}
			}
		}

		public struct Filter
		{
			public EntityRef Entity;
			public AttackRange* AttackRange;
			public PlayerActionState* ActionState;
		}
	}
}