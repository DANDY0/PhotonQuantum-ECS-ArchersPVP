using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.StatusVisuals
{
	[Preserve]
	public unsafe class StatusVisualsFeature : SystemGroup
	{
		public StatusVisualsFeature() : base(nameof(StatusVisualsFeature), CreateSystems())
		{
		}

		private static SystemBase[] CreateSystems()
		{
			return new SystemBase[]
			{
				new ApplyFreezeVisualsSystem(),
				new ApplyFireVisualsSystem(),
				new ApplyPoisonVisualsSystem(),
			};
		}
	}
}