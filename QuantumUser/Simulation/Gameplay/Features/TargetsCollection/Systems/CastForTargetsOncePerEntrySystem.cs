using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.QuantumUser.Simulation.Common.Extensions;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection.Systems
{
public unsafe class CastForTargetsOncePerEntrySystem : SystemMainThread
{
    public override void Update(Frame f)
    {
        var filter = f.Filter<
            ReadyToCollectTargets,
            Radius,
            TargetBuffer,
            TargetLayerMask,
            TargetRelations,
            Owner,
            WorldPosition,
            TargetsHitCooldown>();

        while (filter.NextUnsafe(
                   out EntityRef entity,
                   out _,
                   out Radius* radius,
                   out TargetBuffer* targetBuffer,
                   out TargetLayerMask* layerMask,
                   out TargetRelations* targetRelations,
                   out Owner* owner,
                   out WorldPosition* worldPosition,
                   out TargetsHitCooldown* cooldown
               ))
        {
            // 1. Get current overlap
            var hitsNow = TargetsInRadius(f, worldPosition->Value, radius->Value, layerMask->Value);
            var relations = f.ResolveList(targetRelations->Value);
            var cooldownList = f.ResolveList(cooldown->Value);
            var targetBufferList = f.ResolveList(targetBuffer->Value);

            // 2. Remove from TargetBuffer those who are still inside
            for (int i = targetBufferList.Count - 1; i >= 0; i--)
            {
                if (cooldownList.Contains(targetBufferList[i]))
                    targetBufferList.RemoveAt(i);
            }

            // 3. Add only new entries to both buffer and cooldown
            foreach (EntityRef target in hitsNow)
            {
                if (!IsValidTarget(f, target, owner, relations))
                    continue;

                if (!cooldownList.Contains(target))
                {
                    f.AddToListComponent(targetBuffer->Value, new List<EntityRef> { target });
                    f.AddToListComponent(cooldown->Value, new List<EntityRef> { target });
                }
            }

            // 4. Remove from cooldown those no longer inside
            for (int i = cooldownList.Count - 1; i >= 0; i--)
            {
                if (!hitsNow.Contains(cooldownList[i]))
                    cooldownList.RemoveAt(i);
            }

            // 5. Optional: remove Ready flag
            if (!f.Has<CollectingTargetsContinuously>(entity))
                f.Remove<ReadyToCollectTargets>(entity);
        }
    }

    private List<EntityRef> TargetsInRadius(Frame f, FPVector3 position, FP radius, int layerMask) =>
        f.Physics3D.OverlapShape(position, FPQuaternion.Identity, Shape3D.CreateSphere(radius), layerMask)
            .ToArray()
            .Select(hit => hit.Entity)
            .ToList();

    private bool IsValidTarget(Frame f, EntityRef target, Owner* owner, QList<ETeamRelation> relations)
    {
        if (relations.Contains(ETeamRelation.Owner) && IsLocal(f, target, owner)) return true;
        if (relations.Contains(ETeamRelation.Enemy) && IsTeamDiffer(f, target, owner)) return true;
        if (relations.Contains(ETeamRelation.Ally) && !IsTeamDiffer(f, target, owner) && !IsLocal(f, target, owner)) return true;
        return false;
    }

    private bool IsLocal(Frame f, EntityRef target, Owner* owner) =>
        f.Get<Owner>(target).Link.Value == owner->Link.Value;

    private bool IsTeamDiffer(Frame f, EntityRef target, Owner* owner) =>
        f.Get<Owner>(target).TeamIndex != owner->TeamIndex;
}
}
