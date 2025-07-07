/*using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets.Systems
{
    [Preserve]
    public unsafe class CurrentTargetSetSystem : SystemMainThreadFilter<CurrentTargetSetSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            var selfEntity = filter.Entity;
            
            if (filter.PlayerLifeState->Value == EPlayerLifeState.Dead)
            {
                if(f.Has<CurrentTarget>(selfEntity))
                    f.Remove<CurrentTarget>(selfEntity);
                return;
            }
            
            PlayerLink? nearestTarget = null;
            FP minDistanceSq = FP.MaxValue;

            ComponentIterator<PlayerLink> componentIterator = f.GetComponentIterator<PlayerLink>();
            
            foreach (EntityComponentPair<PlayerLink> enemyEntity in componentIterator)
            {
                if (enemyEntity.Entity.Equals(selfEntity))
                    continue;

                PlayerLifeState enemyLifeState = f.Get<PlayerLifeState>(enemyEntity.Entity);
                if (enemyLifeState.Value == EPlayerLifeState.Dead)
                {
                    if(f.Has<CurrentTarget>(selfEntity))
                        f.Remove<CurrentTarget>(selfEntity);
                    continue;
                }

                var enemyPosition = f.Get<Transform3D>(enemyEntity.Entity).Position;
                FP distanceSq = (filter.Transform3D->Position - enemyPosition).SqrMagnitude;

                if (distanceSq < minDistanceSq)
                {
                    minDistanceSq = distanceSq;
                    nearestTarget = enemyEntity.Component;
                }
                
                if (nearestTarget.HasValue)
                    f.Set(selfEntity, new CurrentTarget { Value = nearestTarget.Value });
                else if (f.Has<CurrentTarget>(selfEntity)) 
                    f.Remove<CurrentTarget>(selfEntity);
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerRef* Ref;
            public PlayerLink* Link;
            public Transform3D* Transform3D;
            public PlayerLifeState* PlayerLifeState;
        }
    }
}*/