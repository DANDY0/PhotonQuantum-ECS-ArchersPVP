using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Bushes.Systems
{
	[Preserve]
	public unsafe class BushRevealSystem : SystemMainThreadFilter<BushRevealSystem.Filter>
	{
		public override void Update(Frame f, ref Filter filter)
		{
			filter.Reveal->Value -= f.DeltaTime;

			if (filter.Reveal->Value <= 0)
				f.Remove<BushTemporaryReveal>(filter.Entity);
		}

		public struct Filter
		{
			public EntityRef Entity;
			public BushTemporaryReveal* Reveal;
		}
	}
}
