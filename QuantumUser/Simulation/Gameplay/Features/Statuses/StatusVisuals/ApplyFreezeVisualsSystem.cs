
namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.StatusVisuals
{
	public class ApplyFreezeVisualsSystem : SystemSignalsOnly, ISignalOnComponentAdded<Freeze>,
		ISignalOnComponentRemoved<Freeze>
	{
		public unsafe void OnAdded(Frame f, EntityRef entity, Freeze* component)
		{
			if (f.Has<Status>(entity) && f.Has<TargetId>(entity))
			{
				var target = f.Get<TargetId>(entity).Value;
				f.Events.FreezeApplied(target);
			}
		}

		public unsafe void OnRemoved(Frame f, EntityRef entity, Freeze* component)
		{
			if (f.Has<Status>(entity) && f.Has<Freeze>(entity) && f.Has<TargetId>(entity))
			{
				var target = f.Get<TargetId>(entity).Value;
				f.Events.FreezeUnApplied(target);
			}
		}
	}
}