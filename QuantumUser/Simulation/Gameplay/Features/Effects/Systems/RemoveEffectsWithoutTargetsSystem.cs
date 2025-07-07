using ModestTree.Util;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Systems
{
    [Preserve]
    public unsafe class RemoveEffectsWithoutTargetsSystem : SystemMainThreadFilter<RemoveEffectsWithoutTargetsSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            EntityRef effect = filter.Entity;
            EntityRef? target = effect.Target(f);
            if (target == null)
                f.Destroy(effect);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Effect* Effect;
            public TargetId* TargetId;
        }
    }
}