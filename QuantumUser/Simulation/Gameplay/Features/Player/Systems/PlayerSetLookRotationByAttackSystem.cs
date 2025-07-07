using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class PlayerSetLookRotationByAttackSystem : SystemMainThreadFilter<PlayerSetLookRotationByAttackSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            if(filter.PlayerLifeState->Value == EPlayerLifeState.Dead)
                return;

            LookDirection* lookDirection = f.Unsafe.GetPointer<LookDirection>(filter.Entity);
            Direction direction = f.Get<Direction>(filter.Entity);

            if(f.Has<CurrentTarget>(filter.Entity))
            {
                var currentTarget = f.Get<CurrentTarget>(filter.Entity).Value.Entity;

                FPVector3 playerPos = filter.Transform3D->Position;
                FPVector3 targetPos = f.Get<Transform3D>(currentTarget).Position;

                FPVector3 directionToTarget = (targetPos - playerPos).Normalized;
                lookDirection->Value = directionToTarget;
            }
            else
            {
                lookDirection->Value = direction.Value;
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform3D;
            public PlayerActionState* PlayerActionState;
            public PlayerLifeState* PlayerLifeState;
            public MovementAvailable* MovementAvailable;
        }
    }
}