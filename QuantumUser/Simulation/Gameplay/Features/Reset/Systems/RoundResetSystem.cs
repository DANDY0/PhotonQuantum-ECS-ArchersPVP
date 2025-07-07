using ModestTree.Util;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.ResetFeature.Systems
{
    [Preserve]
    public unsafe class RoundResetSystem: SystemSignalsOnly,
        ISignalOnRoundStartCountDown,
        ISignalOnGameRunning
    {
        public void OnRoundStartCountDown(Frame f)
        {
            var players = f.Filter<PlayerLink>();
            var weapons = f.Filter<WeaponId>();
            
            while (players.NextUnsafe(
                       out EntityRef entity,
                       out PlayerLink* _))
            {
                if (!f.Has<Dummy>(entity))
                {
                    RemoveArmaments(f, entity);
                    
                    f.Remove<MovementAvailable>(entity);
                    Debug.Log($"MovementAvailable removed from {entity.Index} ");
                    f.Remove<WeaponsActive>(entity);
                    Debug.Log($"WeaponsActive removed from {entity.Index} ");
                }
            }
            
            while (weapons.NextUnsafe(
                       out EntityRef entity,
                       out WeaponId* _))
            {
                if (f.Has<OrbitalShotWeapon>(entity))
                {
                    f.Add<CooldownUp>(entity);
                    Debug.Log($"CooldownUp added for {entity} ");
                }
            }
        }

        public void OnGameRunning(Frame f)
        {
            var players = f.Filter<PlayerLink>();

            while (players.NextUnsafe(
                       out EntityRef entity,
                       out PlayerLink* playerLink))
            {
                if (!f.Has<Dummy>(playerLink->Entity))
                {
                    f.Add<MovementAvailable>(playerLink->Entity);
                    f.Add<WeaponsActive>(entity);
                    
                    Debug.Log($"Player {playerLink->Entity.Index} reset ");
                }
            }
        }
        
        private void RemoveArmaments(Frame f, EntityRef entity)
        {
            foreach (var pair in f.GetComponentIterator<Armament>())
            {
                EntityRef producerID = f.Get<ProducerId>(pair.Entity).Value;
                
                if (producerID.Equals(entity)) 
                    f.Destroy(pair.Entity);
            }
        }
    }
}