using System.Collections.Generic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Lifetime.Systems
{
    public unsafe class UnapplyStatusesOfDeadTargetSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var statuses = new List<StatusEntry>();
            var statusFilter = f.Filter<Status, TargetId>();
            while (statusFilter.NextUnsafe(out EntityRef entity, out Status* _, out TargetId* targetId))
            {
                statuses.Add(new StatusEntry
                {
                    Entity = entity,
                    TargetId = targetId
                });
            }

            var deadEntities = new List<DeadEntry>();
            var deadFilter = f.Filter<Dead>();
            while (deadFilter.NextUnsafe(out EntityRef entity, out Dead* _))
            {
                deadEntities.Add(new DeadEntry
                {
                    Entity = entity
                });
            }

            foreach (var status in statuses)
            {
                foreach (var dead in deadEntities)
                {
                    if (status.TargetId->Value == dead.Entity)
                    {
                        f.Add<Unapplied>(status.Entity);
                        break;
                    }
                }
            }
        }

        private struct StatusEntry
        {
            public EntityRef Entity;
            public TargetId* TargetId;
        }

        private struct DeadEntry
        {
            public EntityRef Entity;
        }
    }
}