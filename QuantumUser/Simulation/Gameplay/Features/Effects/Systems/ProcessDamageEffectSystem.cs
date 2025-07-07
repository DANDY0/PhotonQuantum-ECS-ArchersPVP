using ModestTree.Util;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Systems
{
    [Preserve]
    public unsafe class ProcessDamageEffectSystem : SystemMainThreadFilter<ProcessDamageEffectSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            EntityRef effect = filter.Entity;
            
            EntityRef? target = effect.Target(f);
            
            if (target == null)
                return;

            f.Add<Processed>(effect);

            if (f.Has<Dead>(target.Value) || f.Unsafe.GetPointer<CurrentHp>(target.Value)->Value <= 0)
                return;

            f.Unsafe.GetPointer<CurrentHp>(target.Value)->Value -= filter.EffectValue->Value;
            
            if (f.Unsafe.GetPointer<CurrentHp>(target.Value)->Value <= 0)
            {
                //to avoid negative values
                f.Unsafe.GetPointer<CurrentHp>(target.Value)->Value = 0;

                //TODO probably we will need factory for it
                CreateKillInfoEntity(f, filter);
            }
            
            f.Events.DamageTaken(target.Value, -filter.EffectValue->Value);
        }

        private void CreateKillInfoEntity(Frame f, Filter filter)
        {
            EntityRef entityRef = f.Create();
            f.Add<Kill>(entityRef);
            f.Set(entityRef, new ProducerId{ Value = filter.ProducerId->Value});
            f.Set(entityRef, new TargetId{ Value = filter.TargetId->Value});
        }

        public struct Filter
        {
            public EntityRef Entity;
            public DamageEffect* Effect;
            public EffectValue* EffectValue;
            public TargetId* TargetId;
            public ProducerId* ProducerId;
        }
    }
}