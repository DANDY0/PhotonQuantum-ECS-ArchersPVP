using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection.Systems
{
    public unsafe class CastForTargetsNoLimitSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var ready = f.Filter<
                ReadyToCollectTargets,
                Radius,
                TargetBuffer,
                TargetLayerMask,
                TargetRelations,
                Owner,
                WorldPosition>
            (without:
                ComponentSet.Create<TargetLimit>());

            while (ready.NextUnsafe(
                       out EntityRef entity,
                       out _,
                       out Radius* radius,
                       out TargetBuffer* targetBuffer,
                       out TargetLayerMask* layerMask,
                       out TargetRelations* targetRelations,
                       out Owner* owner,
                       out WorldPosition* worldPosition
                   ))
            {
                List<EntityRef> targets = GetFilteredTargets(f, worldPosition->Value, radius->Value, layerMask->Value,
                    f.ResolveList(targetRelations->Value), owner);

                f.AddToListComponent(targetBuffer->Value, targets);

                if (!f.Has<CollectingTargetsContinuously>(entity))
                    f.Remove<ReadyToCollectTargets>(entity);
            }
        }

        private List<EntityRef> GetFilteredTargets(Frame f, FPVector3 position, FP radius, int layerMask,
            QList<ETeamRelation> targetRelations, Owner* owner)
        {
            List<EntityRef> targetsInRadius = TargetsInRadius(f, position, FPQuaternion.Identity, radius, layerMask);

            List<EntityRef> targets = new List<EntityRef>();

            if (targetRelations.Contains(ETeamRelation.Owner))
                targets.AddRange(targetsInRadius.Where(target => IsLocal(f, target, owner)));

            if (targetRelations.Contains(ETeamRelation.Enemy))
                targets.AddRange(targetsInRadius.Where(target => IsTeamDiffer(f, target, owner)));

            if (targetRelations.Contains(ETeamRelation.Ally))
                targets.AddRange(targetsInRadius.Where(target =>
                    !IsTeamDiffer(f, target, owner) && !IsLocal(f, target, owner)));

            return targets;
        }

        private List<EntityRef> TargetsInRadius(Frame f, FPVector3 position, FPQuaternion rotation, FP radius,
            int layerMask) =>
            f.Physics3D.OverlapShape(
                    position,
                    rotation,
                    Shape3D.CreateSphere(radius),
                    layerMask
                )
                .ToArray()
                .Select(x => x.Entity)
                .ToList();

        private bool IsLocal(Frame f, EntityRef target, Owner* owner) =>
            f.Get<Owner>(target).Link.Value == owner->Link.Value;

        private bool IsTeamDiffer(Frame f, EntityRef target, Owner* owner) =>
            f.Get<Owner>(target).TeamIndex != owner->TeamIndex;
    }
}