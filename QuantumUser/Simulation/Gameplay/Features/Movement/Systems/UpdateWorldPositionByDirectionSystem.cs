using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems
{
    [Preserve]
    public unsafe class UpdateWorldPositionByDirectionSystem : SystemMainThreadFilter<UpdateWorldPositionByDirectionSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        { 
            var newPosition = filter.WorldPosition->Value + filter.Direction->Value 
                * filter.Speed->Value * f.DeltaTime;
            
            f.Set(filter.Entity, new WorldPosition { Value = newPosition });
        }
        
        public struct Filter
        {
            public EntityRef Entity;
            public WorldPosition* WorldPosition;
            public Direction* Direction;
            public Speed* Speed;
            public MovementAvailable* MovementAvailable;
            public Moving* Moving;
        }
    }
}