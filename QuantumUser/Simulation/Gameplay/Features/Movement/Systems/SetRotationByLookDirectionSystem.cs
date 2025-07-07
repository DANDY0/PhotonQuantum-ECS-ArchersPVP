using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems
{
    //for when player idle, look at the target
    [Preserve]
    public unsafe class SetRotationByLookDirectionSystem : SystemMainThreadFilter<SetRotationByLookDirectionSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            if (filter.PlayerActionState->Value == EPlayerActionState.Moving)
                return;
            
            LookDirection* lookDirection = f.Unsafe.GetPointer<LookDirection>(filter.Entity);

            //means no target to look at
            if (lookDirection->Value == FPVector3.Zero)
                return;

            FPVector3 forward = lookDirection->Value.Normalized;
            FPQuaternion targetRotation = FPQuaternion.LookRotation(forward, FPVector3.Up);

            filter.Transform3D->Rotation = FPQuaternion.Slerp(
                filter.Transform3D->Rotation, 
                targetRotation, 
                f.DeltaTime * 15
            );
        }

        public struct Filter
        {
            public EntityRef Entity;
            public LookDirection* LookDirection;
            public Transform3D* Transform3D;
            public PlayerActionState* PlayerActionState;
            public MovementAvailable* MovementAvailable;
        }
    }
}