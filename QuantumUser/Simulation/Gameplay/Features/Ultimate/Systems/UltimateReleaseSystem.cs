using ModestTree.Util;
using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Ultimate.Factory;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Ultimate.Systems
{
    [Preserve]
    public unsafe class UltimateReleaseSystem : SystemMainThreadFilter<UltimateReleaseSystem.Filter>
    {
        private IUltimateFactory _ultimateFactory;

        public override void OnInit(Frame f)
        {
            _ultimateFactory = DI.Resolve<IUltimateFactory>();
        }
        
        public override void Update(Frame f, ref Filter filter)
        {
            Input* input = f.GetPlayerInput(filter.Link->Value);

            if (input->IsUltimate)
            {
                input->IsUltimate = false;

                Owner owner = f.Get<Owner>(filter.Link->Entity);
                EUltimateId ultimateId = f.Get<UltimateId>(filter.Link->Entity).Value;

                FPVector3 finalDirection = input->FinalUltDirection;

                if (owner.TeamIndex != 0)
                {
                    finalDirection = new FPVector3(
                        -finalDirection.X,
                        finalDirection.Y,
                        -finalDirection.Z
                    );
                }

                f.Add<UltProcessing>(filter.Link->Entity);
                _ultimateFactory.CreateBasicUltimate(f, 1, owner, ultimateId, finalDirection);
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public PlayerLink* Link;
        }
    }
}