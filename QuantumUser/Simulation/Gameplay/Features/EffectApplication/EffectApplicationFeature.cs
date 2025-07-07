using Quantum.QuantumUser.Simulation.Gameplay.Features.EffectApplication.Systems;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.EffectApplication
{
    public class EffectApplicationFeature : SystemGroup
    {
        public EffectApplicationFeature() : base(nameof(EffectApplicationFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new ApplyEffectsOnTargetsSystem(),
                new ApplyStatusesOnTargetsSystem(),
            };
        }
    }
}