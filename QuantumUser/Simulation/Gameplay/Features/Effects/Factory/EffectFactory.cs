using System;
using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Extensions;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Factory
{
    public class EffectFactory : IEffectFactory
    {
        public EntityRef CreateEffect(Frame f, EffectSetup setup, EntityRef producerId, EntityRef targetId, Owner owner)
        {
            switch (setup.EffectTypeId)
            {
                case EffectTypeId.Unknown:
                    break;
                case EffectTypeId.Damage:
                    return CreateDamage(f, producerId, targetId, setup.Value, owner);
                case EffectTypeId.Heal:
                    return CreateHeal(f, producerId, targetId, setup.Value, owner);
            }

            throw new Exception($"Effect with type id {setup.EffectTypeId} does not exist");
        }
        
        private EntityRef CreateDamage(Frame f, EntityRef producerId, EntityRef targetId, FP value, Owner owner)
        {
            EntityRef entity = f.CreateEmpty(owner);

            f.Add<Effect>(entity);
            f.Add<DamageEffect>(entity);
            f.Set(entity, new EffectValue { Value = value });
            f.Set(entity, new ProducerId { Value = producerId });
            f.Set(entity, new TargetId { Value = targetId });

            return entity;
        }
        
        private EntityRef CreateHeal(Frame f, EntityRef producerId, EntityRef targetId, FP value, Owner owner)
        {
            EntityRef entity = f.CreateEmpty(owner);

            f.Add<Effect>(entity);
            f.Add<HealEffect>(entity);
            f.Set(entity, new EffectValue { Value = value });
            f.Set(entity, new ProducerId { Value = producerId });
            f.Set(entity, new TargetId { Value = targetId });

            return entity;
        }
    }
}