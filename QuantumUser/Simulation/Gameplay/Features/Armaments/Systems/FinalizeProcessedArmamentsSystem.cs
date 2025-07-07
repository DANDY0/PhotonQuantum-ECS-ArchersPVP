using ModestTree.Util;
using Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Systems
{
    [Preserve]
    public unsafe class FinalizeProcessedArmamentsSystem : SystemMainThreadFilter<FinalizeProcessedArmamentsSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            filter.Armament.RemoveTargetCollectionComponents(f);
            f.Add<Destructed>(filter.Armament);
        }

        public struct Filter
        {
            public EntityRef Armament;
            public Armament* IsArmament;
            public Processed* Processed;
        }
    }
}