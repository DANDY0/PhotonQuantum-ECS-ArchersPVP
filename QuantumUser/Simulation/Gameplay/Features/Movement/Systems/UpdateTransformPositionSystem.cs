using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems
{
    [Preserve]
    public unsafe class UpdateTransformPositionSystem : SystemMainThreadFilter<UpdateTransformPositionSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            f.Unsafe.GetPointer<Transform3D>(filter.Entity)->Position = filter.WorldPosition->Value;
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Transform3D* Transform3D;
            public WorldPosition* WorldPosition;
        }
    }
}