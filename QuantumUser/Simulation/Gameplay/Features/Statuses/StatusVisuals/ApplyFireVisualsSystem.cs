using Quantum;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Effects;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.StatusVisuals
{
	public class ApplyFireVisualsSystem : SystemSignalsOnly, ISignalOnComponentAdded<Fire>,
		ISignalOnComponentRemoved<Fire>
	{
		public unsafe void OnAdded(Frame f, EntityRef entity, Fire* component)
		{
			if (f.Has<Status>(entity) && f.Has<TargetId>(entity))
			{
				var target = f.Get<TargetId>(entity).Value;
				f.Events.FireApplied(target);
			}
		}

		public unsafe void OnRemoved(Frame f, EntityRef entity, Fire* component)
		{
			if (f.Has<Status>(entity) && f.Has<TargetId>(entity))
			{
				var target = f.Get<TargetId>(entity).Value;
				f.Events.FireUnApplied(target);
			}
		}
	}
}