using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Systems
{

    [Preserve]
    public unsafe class StatusDurationSystem : SystemMainThreadFilter<StatusDurationSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            if (filter.TimeLeft->Value >= 0)
                f.Set(filter.Entity, new TimeLeft { Value = filter.TimeLeft->Value - f.DeltaTime });
            else
                f.Add<Unapplied>(filter.Entity);

        }

        public struct Filter
        {
            public EntityRef Entity;
            public Duration* Duration;
            public Status* Status;
            public TimeLeft* TimeLeft;
        }
    }
}