using Quantum.QuantumUser.Simulation.Gameplay.Features.Ultimate.Systems;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Ultimate
{
    public class UltimateFeature : SystemGroup
    {
        public UltimateFeature() : base(nameof(UltimateFeature), CreateSystems())
        {
            
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new UltimateReleaseSystem(),
                new BasicUltimateSystem(),
            };
        }
    }
}