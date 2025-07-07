using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Systems
{
    [Preserve]
    public unsafe class CleanupUnappliedStatuses : SystemMainThreadFilter<CleanupUnappliedStatuses.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            f.Add<Destructed>(filter.Entity);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Status* Status;
            public Unapplied* UnApplied;
        }
    }
    
}