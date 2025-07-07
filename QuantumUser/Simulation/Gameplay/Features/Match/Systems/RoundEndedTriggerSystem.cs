using UnityEngine;
using UnityEngine.Scripting;
using System.Collections.Generic;

namespace Quantum.Gameplay.Features.Match
{
    [Preserve]
    public unsafe class RoundEndedTriggerSystem : SystemSignalsOnly, ISignalPlayerDead
    {
        public void PlayerDead(Frame f, Owner deadPlayer)
        {
            var kills = f.Filter<Kill>();
            List<EntityRef> toRemove = new();

            while (kills.NextUnsafe(
                       out EntityRef entity,
                       out Kill* kill))
            {
                if (f.Get<TargetId>(entity).Value == deadPlayer.Link.Entity)
                {
                    ProducerId producerId = f.Get<ProducerId>(entity);
                    Owner ownerKiller = f.Get<Owner>(producerId.Value); 
                    
                    f.Global->MatchScore[ownerKiller.TeamIndex]++;
                    
                    //TODO isDraw is false temporary, add logic for draw
                    f.Signals.OnRoundEnded(ownerKiller, deadPlayer, false);
                    Debug.Log("Player OnRoundEnded SIGNAL");

                    toRemove.Add(entity);
                }
            }

            foreach (var entity in toRemove) 
                f.Destroy(entity);
        }
    }
}