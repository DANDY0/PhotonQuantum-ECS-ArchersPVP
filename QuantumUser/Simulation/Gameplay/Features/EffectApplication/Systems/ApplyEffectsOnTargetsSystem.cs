using Quantum.Collections;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Factory;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.EffectApplication.Systems
{
    [Preserve]
    public unsafe class ApplyEffectsOnTargetsSystem : SystemMainThreadFilter<ApplyEffectsOnTargetsSystem.Filter>
    {
        private IEffectFactory _effectFactory;

        public override void OnInit(Frame f)
        {
            _effectFactory = DI.Resolve<IEffectFactory>();
        }

        public override void Update(Frame f, ref Filter filter)
        {
            QList<EntityRef> targetsBuffer = f.ResolveList(filter.TargetsBuffer->Value);
            QList<EffectSetup> effectSetups = f.ResolveList(filter.EffectSetups->Value);
            
            foreach (EntityRef target in targetsBuffer)
            foreach (EffectSetup effectSetup in effectSetups)
            {
                _effectFactory
                    .CreateEffect(f,
                        effectSetup, ProducerId(f, filter.Entity), target, 
                    f.Get<Owner>(filter.Entity));
            }
        }
        
        private EntityRef ProducerId(Frame f, EntityRef entity)
        {
            return f.Has<ProducerId>(entity) ? f.Get<ProducerId>(entity).Value : entity;
        }

        public struct Filter
        {
            public EntityRef Entity;
            public TargetBuffer* TargetsBuffer;
            public EffectSetups* EffectSetups;
        }
    }
}