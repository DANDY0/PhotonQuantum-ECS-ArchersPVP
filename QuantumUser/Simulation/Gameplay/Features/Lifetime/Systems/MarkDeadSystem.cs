using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Lifetime.Systems
{
    public unsafe class MarkDeadSystem : SystemMainThread
    {
        public override void Update(Frame f)
        {
            var entities = f.Filter<CurrentHp, MaxHp>(
                without: ComponentSet.Create<Dead>()
            );

            while (entities.NextUnsafe(
                       out EntityRef entity, 
                       out CurrentHp* currentHp, 
                       out _))
            {
                if (currentHp->Value <= 0)
                {
                    f.Add<Dead>(entity);
                    f.Add<ProcessingDeath>(entity);
                    
                    f.Signals.PlayerDead(f.Get<Owner>(entity));
                    Debug.Log("Player Dead SIGNAL");
                }
            }
        }
    }
}