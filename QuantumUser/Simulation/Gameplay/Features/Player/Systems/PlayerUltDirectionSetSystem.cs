using Photon.Deterministic;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class PlayerUltDirectionSetSystem : SystemMainThreadFilter<PlayerUltDirectionSetSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            if (filter.PlayerLifeState->Value == EPlayerLifeState.Dead)
            {
                f.Unsafe.GetPointer<Direction>(filter.Entity)->Value = FPVector3.Zero;
                return;
            }

            Input* input = f.GetPlayerInput(filter.Link->Value);

            FPVector3 ultDirection = input->UltDirection;

            if (filter.Owner->TeamIndex != 0)
            {
                ultDirection = new FPVector3(
                    -ultDirection.X,
                    ultDirection.Y,
                    -ultDirection.Z
                );
            }

            if (ultDirection != FPVector3.Zero)
                ultDirection = ultDirection.Normalized;

            f.Unsafe.GetPointer<UltDirection>(filter.Entity)->Value = ultDirection;
        }


        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* Link;
            public Owner* Owner;
            public PlayerLifeState* PlayerLifeState;
        }
    }
}