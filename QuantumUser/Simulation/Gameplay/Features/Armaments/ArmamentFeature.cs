using Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Systems;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments
{
    public class ArmamentFeature : SystemGroup
    {
        public ArmamentFeature() : base(nameof(ArmamentFeature), CreateSystems())
        {
            
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new MarkProcessedOnTargetLimitExceededSystem(),
                new FollowProducerSystem(),

                new FinalizeProcessedArmamentsSystem(),
            };
        }
    }
}