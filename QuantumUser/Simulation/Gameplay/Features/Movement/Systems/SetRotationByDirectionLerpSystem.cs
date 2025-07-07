using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems
{
    //for objects to rotate towards current direction
    [Preserve]
    public unsafe class SetRotationByDirectionLerpSystem : SystemMainThreadFilter<SetRotationByDirectionLerpSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            Direction* direction = f.Unsafe.GetPointer<Direction>(filter.Entity);
            Transform3D* transform = f.Unsafe.GetPointer<Transform3D>(filter.Entity);

            if (direction->Value == FPVector3.Zero)
                return;

            FPVector3 targetDirection = direction->Value.Normalized;

            FPQuaternion targetRotation = FPQuaternion.LookRotation(targetDirection, FPVector3.Up);
            //TODO move speed to config 
            transform->Rotation = FPQuaternion.Slerp(
                transform->Rotation,
                targetRotation,
                f.DeltaTime * filter.RotationSpeed->Value);
        }

        public struct Filter
        { 
            public EntityRef Entity;
            public Transform3D* Transform;
            public Direction* Direction;
            public MovementAvailable* MovementAvailable;
            public RotationSpeed* RotationSpeed;
        }
    }
}