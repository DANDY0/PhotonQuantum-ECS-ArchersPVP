using System;
using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class AnimationStatesEventsSystem : SystemMainThreadFilter<AnimationStatesEventsSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        { 
            
            switch (filter.PlayerActionState->Value)
            {
                case EPlayerActionState.None:
                    break;
                case EPlayerActionState.Idle:
                    f.Events.PlayerIdle(filter.Entity);
                    break;
                case EPlayerActionState.Moving:
                    f.Events.PlayerRun(filter.Entity);
                    break;
                case EPlayerActionState.Attacking:
                    f.Events.PlayerIdle(filter.Entity);
                    break;
                case EPlayerActionState.AttackPreparing:
                    f.Events.PlayerIdle(filter.Entity);
                    break;
                case EPlayerActionState.UltAiming:
                    f.Events.PlayerIdle(filter.Entity);
                    break;
                case EPlayerActionState.UltProcessing:
                    f.Events.PlayerIdle(filter.Entity);
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* Link;
            public PlayerActionState* PlayerActionState;
        }

    }
}
