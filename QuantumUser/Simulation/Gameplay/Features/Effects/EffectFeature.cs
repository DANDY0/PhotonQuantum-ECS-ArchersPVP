using Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Systems;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects
{
    public class EffectFeature : SystemGroup
    {
        public EffectFeature() : base(nameof(EffectFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new RemoveEffectsWithoutTargetsSystem(),

                new ProcessDamageEffectSystem(),
                new ProcessHealEffectSystem(),

                new CleanupProcessedEffects(),
            };
        }
    }
}