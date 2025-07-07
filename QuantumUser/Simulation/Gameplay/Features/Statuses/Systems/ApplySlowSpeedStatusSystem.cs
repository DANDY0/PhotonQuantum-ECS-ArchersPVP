using Quantum.Collections;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Systems
{
    [Preserve]
    public unsafe class ApplySlowSpeedStatusSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var without = ComponentSet.Create<Affected>();
            var statuses = f.Filter<
                Status,
                StatusEffectTypeId,
                ProducerId,
                TargetId,
                EffectValue>(without);

            QListPtr<EntityRef> affectedEntities = f.AllocateList<EntityRef>();

            while (statuses.NextUnsafe(
                       out EntityRef statusRef,
                       out Status* status,
                       out StatusEffectTypeId* statusEffectTypeId,
                       out ProducerId* producerId,
                       out TargetId* targetId,
                       out EffectValue* effectValue))
            {
                if(statusEffectTypeId->Value != EStatusEffectTypeId.SlowSpeed)
                    continue;
                
                EntityRef statusEntity = f.Create();
                f.Set(statusEntity, new StatChange { Value = EStats.Speed });
                f.Set(statusEntity, new TargetId { Value = targetId->Value });
                f.Set(statusEntity, new ProducerId { Value = producerId->Value });
                f.Set(statusEntity, new EffectValue { Value = effectValue->Value });
                f.Set(statusEntity, new ApplierStatusLink { Value = statusEntity.Index });

                f.ResolveList(affectedEntities).Add(statusRef);
            }

            var list = f.ResolveList(affectedEntities);
            for (int i = 0; i < list.Count; i++)
                f.Set(list[i], new Affected());
        }
    }
}