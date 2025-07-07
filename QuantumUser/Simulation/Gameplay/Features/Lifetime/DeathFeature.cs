using Quantum.QuantumUser.Simulation.Gameplay.Features.Lifetime.Systems;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Lifetime
{
    public class DeathFeature : SystemGroup
    {
        public DeathFeature() : base(nameof(DeathFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new MarkDeadSystem(),
                new UnapplyStatusesOfDeadTargetSystem(),
                new KillInfoSystem(),
            };
        }
    }
}