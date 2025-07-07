using ModestTree;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class PlayerDeathSystem : SystemMainThreadFilter<PlayerDeathSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            f.Set(filter.Entity, new PlayerLifeState { Value = EPlayerLifeState.Dead });

            BlockMovement(f, filter);
            RemoveWeapons(f, filter);
            RemoveArmaments(f, filter);

            f.Events.PlayerDead(filter.Entity);
        }

        private void RemoveArmaments(Frame f, Filter filter)
        {
            foreach (var pair in f.GetComponentIterator<Armament>())
            {
                EntityRef producerID = f.Get<ProducerId>(pair.Entity).Value;
                
                if (producerID.Equals(filter.PlayerLink->Entity)) 
                    f.Destroy(pair.Entity);
            }
        }

        private void RemoveWeapons(Frame f, Filter filter)
        {
            foreach (var pair in f.GetComponentIterator<WeaponId>())
            {
                EntityRef producerEntity = f.Get<ProducerId>(pair.Entity).Value;
                
                if (producerEntity.Equals(filter.PlayerLink->Entity)) 
                    f.Remove<WeaponsActive>(producerEntity);
            }
        }

        private void BlockMovement(Frame f, Filter filter)
        {
            f.Remove<MovementAvailable>(filter.Entity);
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* PlayerLink;
            public Dead* Dead;
            public ProcessingDeath* ProcessingDeath;
            public PhysicsCollider3D* PhysicsCollider3D;
        }
    }

}