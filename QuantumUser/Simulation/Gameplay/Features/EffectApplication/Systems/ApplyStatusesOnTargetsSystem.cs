using Quantum.Collections;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Applier;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.EffectApplication.Systems
{
    [Preserve]
    public unsafe class ApplyStatusesOnTargetsSystem : SystemMainThreadFilter<ApplyStatusesOnTargetsSystem.Filter>
    {
        private IStatusApplier _statusApplier;

        public override void OnInit(Frame f)
        {
            _statusApplier = DI.Resolve<IStatusApplier>();
        }
        
        public override void Update(Frame f, ref Filter filter)
        {
            // Debug.Log("ApplyStatus 1");
            QList<EntityRef> targetsBuffer = f.ResolveList(filter.TargetsBuffer->Value);
            QList<StatusSetup> statusSetups = f.ResolveList(filter.StatusSetups->Value);
            
            foreach (EntityRef target in targetsBuffer)
            foreach (StatusSetup setup in statusSetups)
            {
                // Debug.Log("ApplyStatus 2");
                _statusApplier.ApplyStatus(f, setup, ProducerId(f, filter.Entity), target);
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
            public StatusSetups* StatusSetups;
        }
    }
}