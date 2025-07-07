using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Systems
{
    [Preserve]
    public unsafe class CleanupUnappliedStatusLinkedChanges : SystemMainThreadFilter<CleanupUnappliedStatusLinkedChanges.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            foreach (var entity in f.GetComponentIterator<ApplierStatusLink>()) 
                f.Add<Destructed>(entity.Entity);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Status* EntityStatus;
            public Unapplied* UnApplied;
        }
    }
}