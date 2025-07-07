using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.Physics3D;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection.Systems
{
    [Preserve]
    public unsafe class CastForTargetsWithLimitSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var ready = f.Filter<
                ReadyToCollectTargets,
                Radius,
                TargetBuffer,
                ProcessedTargets,
                TargetLimit,
                WorldPosition,
                TargetLayerMask,
                TargetRelations>();

            while (ready.NextUnsafe(
                       out EntityRef entity,
                       out _,
                       out Radius* radius,
                       out TargetBuffer* targetBuffer,
                       out ProcessedTargets* processedTargets,
                       out TargetLimit* targetLimit,
                       out WorldPosition* worldPosition,
                       out TargetLayerMask* layerMask,
                       out TargetRelations* targetRelations))
            {

                Owner owner = f.Get<Owner>(entity);
                Hit3D[] hits = TargetCountInRadius(f, worldPosition->Value, FPQuaternion.Identity,
                    radius->Value, layerMask->Value, owner, targetRelations);

                for (int i = 0; i < FPMath.Min(hits.Length, targetLimit->Value); i++)
                {
                    EntityRef targetId = hits[i].Entity;

                    if (!AlreadyProcessed(f.ResolveList(processedTargets->Value), targetId) &&
                        IsValidTarget(f, targetId, f.Get<Owner>(entity), targetRelations))
                    {
                        f.AddToListComponent(targetBuffer->Value, new List<EntityRef> { targetId });
                        f.AddToListComponent(processedTargets->Value, new List<EntityRef> { targetId });
                    }
                }

                if (!f.Has<CollectingTargetsContinuously>(entity))
                    f.Remove<ReadyToCollectTargets>(entity);
            }
        }

        private bool AlreadyProcessed(QList<EntityRef> processedTargets, EntityRef targetId) => 
            processedTargets.Contains(targetId);

        private Hit3D[] TargetCountInRadius(Frame f, FPVector3 position, FPQuaternion rotation, FP radius,
            int layerMask, Owner owner, TargetRelations* targetRelations) =>
            f.Physics3D.OverlapShape(
                    position,
                    rotation,
                    Shape3D.CreateSphere(radius),
                    layerMask
                )
                .ToArray()
                .Where(hit => IsValidTarget(f, hit.Entity, owner, targetRelations))
                .ToArray();

        private bool IsValidTarget(Frame f, EntityRef target, Owner owner, TargetRelations* targetRelations)
        {
            if (f.ResolveList(targetRelations->Value).Contains(ETeamRelation.Owner) && IsLocal(f, target, owner))
                return true;

            if (f.ResolveList(targetRelations->Value).Contains(ETeamRelation.Enemy) && IsTeamDiffer(f, target, owner))
                return true;

            if (f.ResolveList(targetRelations->Value).Contains(ETeamRelation.Ally) && !IsTeamDiffer(f, target, owner) &&
                !IsLocal(f, target, owner))
                return true;

            return false;
        }

        private bool IsLocal(Frame f, EntityRef target, Owner owner) =>
            f.Get<Owner>(target).Link.Value == owner.Link.Value;

        private bool IsTeamDiffer(Frame f, EntityRef target, Owner owner) =>
            f.Get<Owner>(target).TeamIndex != owner.TeamIndex;
    }
}