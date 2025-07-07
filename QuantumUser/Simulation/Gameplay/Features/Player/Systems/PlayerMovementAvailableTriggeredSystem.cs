using ModestTree.Util;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class PlayerMovementAvailableTriggeredSystem: SystemSignalsOnly, 
        ISignalOnGameRunning,
        ISignalOnRoundStartCountDown,
        ISignalOnComponentRemoved<MovementAvailable>,
        ISignalOnComponentAdded<MovementAvailable>
    {
        public void OnGameRunning(Frame f)
        {
            /*var players = f.Filter<PlayerLink>();

            while (players.NextUnsafe(
                       out EntityRef entity,
                       out PlayerLink* playerLink))
            {
                if (!f.Has<Dummy>(playerLink->Entity))
                {
                    f.Add<MovementAvailable>(playerLink->Entity);
                    Debug.Log($"MovementAvailable added for {playerLink->Entity.Index} ");
                }
            }*/
        }

        public void OnRoundStartCountDown(Frame f)
        {
            /*var players = f.Filter<PlayerLink>();

            while (players.NextUnsafe(
                       out EntityRef entity,
                       out PlayerLink* playerLink))
            {
                if (!f.Has<Dummy>(playerLink->Entity) && f.Has<MovementAvailable>(playerLink->Entity))
                {
                    f.Remove<MovementAvailable>(playerLink->Entity);
                    Debug.Log($"MovementAvailable added for {playerLink->Entity.Index} ");
                }
            }*/
        }

        public void OnAdded(Frame f, EntityRef entity, MovementAvailable* component)
        {
            /*if (!f.Has<WeaponsActive>(entity))
                f.Add<WeaponsActive>(entity);*/
        }

        public void OnRemoved(Frame f, EntityRef entity, MovementAvailable* component)
        {
            /*if (f.DestroyPending(entity))
                return;
            
            if (f.Has<WeaponsActive>(entity))
                f.Remove<WeaponsActive>(entity);*/
        }
    }
}