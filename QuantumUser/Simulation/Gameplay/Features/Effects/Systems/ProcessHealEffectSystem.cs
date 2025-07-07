using ModestTree.Util;
using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Systems
{
    [Preserve]
    public unsafe class ProcessHealEffectSystem : SystemMainThreadFilter<ProcessHealEffectSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            EntityRef effect = filter.Entity;
            
            EntityRef? target = effect.Target(f);

            if (target == null)
                return;
            
            f.Add<Processed>(effect);

            if (f.Has<Dead>(target.Value))
                return;

            if (f.Has<CurrentHp>(target.Value) && f.Has<MaxHp>(target.Value))
            {
                var newValue = FPMath.Min(f.Get<CurrentHp>(target.Value).Value + filter.EffectValue->Value, f.Get<MaxHp>(target.Value).Value);
                f.Set(target.Value, new CurrentHp { Value = newValue });
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public HealEffect* Effect;
            public EffectValue* EffectValue;
            public TargetId* TargetId;
        }
    }
}