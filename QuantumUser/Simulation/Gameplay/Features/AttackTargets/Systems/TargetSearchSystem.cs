using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets.Systems
{
    [Preserve]
    public unsafe class TargetSearchSystem : SystemMainThreadFilter<TargetSearchSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            var selfEntity = filter.Entity;

            if (filter.PlayerLifeState->Value == EPlayerLifeState.Dead)
            {
                if (f.Has<CurrentTarget>(selfEntity))
                    f.Remove<CurrentTarget>(selfEntity);
                return;
            }

            PlayerLink? nearestTarget = null;
            FP minDistanceSq = FP.MaxValue;

            var range = filter.AttackRange->CurrentRange;
            var rangeSq = range * range;
            var selfPosition = filter.Transform3D->Position;

            foreach (var enemy in f.GetComponentIterator<PlayerLink>())
            {
                var enemyEntity = enemy.Entity;

                if (enemyEntity.Equals(selfEntity))
                    continue;

                if (!f.Has<IsVisibleToEnemies>(enemyEntity))
                    continue;

                var enemyLifeState = f.Get<PlayerLifeState>(enemyEntity);
                if (enemyLifeState.Value == EPlayerLifeState.Dead)
                    continue;
                
                var enemyPos = f.Get<Transform3D>(enemyEntity).Position;
                FP distanceSq = (selfPosition - enemyPos).SqrMagnitude;

                if (distanceSq <= rangeSq && distanceSq < minDistanceSq)
                {
                    minDistanceSq = distanceSq;
                    nearestTarget = enemy.Component;
                }
            }

            if (nearestTarget.HasValue)
                f.Set(selfEntity, new CurrentTarget { Value = nearestTarget.Value });
            else if (f.Has<CurrentTarget>(selfEntity))
                f.Remove<CurrentTarget>(selfEntity);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* Link;
            public Transform3D* Transform3D;
            public PlayerLifeState* PlayerLifeState;
            public AttackRange* AttackRange;
        }
    }
}
