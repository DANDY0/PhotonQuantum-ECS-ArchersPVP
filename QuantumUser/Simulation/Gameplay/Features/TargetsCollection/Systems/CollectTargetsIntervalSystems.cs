using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection.Systems
{
    [Preserve]
    public unsafe class CollectTargetsIntervalSystems : SystemMainThreadFilter<CollectTargetsIntervalSystems.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            var collectTargetsTimer = f.Unsafe.GetPointer<CollectTargetsTimer>(filter.Entity);
            
            collectTargetsTimer->Value -= f.DeltaTime;

            if (collectTargetsTimer->Value <= 0)
            {
                f.Add<ReadyToCollectTargets>(filter.Entity);
                collectTargetsTimer->Value = filter.CollectTargetsInterval->Value;
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public TargetBuffer* TargetBuffer;
            public CollectTargetsInterval* CollectTargetsInterval;
            public CollectTargetsTimer* CollectTargetsTimer;
        }
    }
}