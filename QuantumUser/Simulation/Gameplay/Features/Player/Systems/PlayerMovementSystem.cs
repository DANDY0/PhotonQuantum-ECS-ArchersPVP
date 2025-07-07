using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class PlayerMovementSystem : SystemMainThreadFilter<PlayerMovementSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            CharacterController3D* characterController3D = f.Unsafe.GetPointer<CharacterController3D>(filter.Entity);
            characterController3D->MaxSpeed = filter.Speed->Value;
            characterController3D->Move(f, filter.Entity, filter.Direction->Value);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform3D;
            public Direction* Direction;
            public Speed* Speed;
            public CharacterController3D* KCC;
            public MovementAvailable* MovementAvailable;
            public PlayerLink* Link;
        }
    }
}