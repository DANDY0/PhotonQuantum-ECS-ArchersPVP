using ModestTree.Util;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems
{
    [Preserve]
    public unsafe class OrbitCenterFollowSystem : SystemMainThreadFilter<OrbitCenterFollowSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            var targets = f.Filter<Transform3D>();

            while (targets.NextUnsafe(
                       out EntityRef target,
                       out Transform3D* transform3D))
            {
                if (filter.OrbitCenterFollowTarget->Value == target)
                    f.Set(filter.Entity, new OrbitCenterPosition { Value = transform3D->Position });
            }
        }
        
        public struct Filter
        {
            public EntityRef Entity;
            public OrbitCenterPosition* OrbitCenterPosition;
            public OrbitCenterFollowTarget* OrbitCenterFollowTarget;
        }
    }
    

}