using ModestTree.Util;
using Quantum.Collections;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Systems
{
    [Preserve]
    public unsafe class MarkProcessedOnTargetLimitExceededSystem : SystemMainThreadFilter<MarkProcessedOnTargetLimitExceededSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            QList<EntityRef> processedTargets = f.ResolveList(filter.ProcessedTargets->Value);
            if (processedTargets.Count >= filter.TargetLimit->Value)
                f.Add<Processed>(filter.Armament);
        }

        public struct Filter
        {
            public EntityRef Armament;
            public Armament* IsArmament;
            public TargetLimit* TargetLimit;
            public ProcessedTargets* ProcessedTargets;
        }
    }
}