using System.Collections.Generic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Systems
{
    public unsafe class FollowProducerSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var followers = new List<FollowerEntry>();
            var followerFilter = f.Filter<FollowingProducer, WorldPosition, ProducerId>();

            while (followerFilter.NextUnsafe(
                       out EntityRef entity,
                       out FollowingProducer* followingProducer,
                       out WorldPosition* followerWorldPosition,
                       out ProducerId* producerId))
            {
                followers.Add(new FollowerEntry
                {
                    Entity = entity,
                    FollowingProducer = followingProducer,
                    FollowerWorldPosition = followerWorldPosition,
                    ProducerId = producerId
                });
            }

            var producers = new List<ProducerEntry>();
            var producerFilter = f.Filter<WorldPosition>();

            while (producerFilter.NextUnsafe(out EntityRef entity, out WorldPosition* producerWorldPosition))
            {
                producers.Add(new ProducerEntry
                {
                    Entity = entity,
                    ProducerWorldPosition = producerWorldPosition
                });
            }

            foreach (FollowerEntry follower in followers)
            {
                foreach (ProducerEntry producer in producers)
                {
                    if (follower.ProducerId->Value == producer.Entity)
                    {
                        follower.FollowerWorldPosition->Value = producer.ProducerWorldPosition->Value;
                        break;
                    }
                }
            }
        }

        private struct FollowerEntry
        {
            public EntityRef Entity;
            public FollowingProducer* FollowingProducer;
            public WorldPosition* FollowerWorldPosition;
            public ProducerId* ProducerId;
        }

        private struct ProducerEntry
        {
            public EntityRef Entity;
            public WorldPosition* ProducerWorldPosition;
        }
    }
}
