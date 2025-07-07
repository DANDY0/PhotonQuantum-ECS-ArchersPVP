namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.StatusVisuals
{
	public class ApplyPoisonVisualsSystem : SystemSignalsOnly, ISignalOnComponentAdded<Poison>,
		ISignalOnComponentRemoved<Poison>
	{
		public unsafe void OnAdded(Frame f, EntityRef entity, Poison* component)
		{
			if (f.Has<Status>(entity) && f.Has<TargetId>(entity))
			{
				var target = f.Get<TargetId>(entity).Value;
				f.Events.PoisonApplied(target);
			}
		}

		public unsafe void OnRemoved(Frame f, EntityRef entity, Poison* component)
		{
			if (f.Has<Status>(entity) && f.Has<TargetId>(entity))
			{
				var target = f.Get<TargetId>(entity).Value;
				f.Events.PoisonUnApplied(target);
			}
		}
	}
}