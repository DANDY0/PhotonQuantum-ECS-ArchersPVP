using Quantum.QuantumUser.Simulation.Gameplay.Features.Cleanup;
using Quantum.Task;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Systems
{
    public unsafe class CleanupProcessedEffects : SystemBase, ICleanupSystem
    {
        protected override TaskHandle Schedule(Frame f, TaskHandle taskHandle) => 
            taskHandle;

        public void Cleanup(Frame f)
        {
            ComponentFilter<Effect, Processed> effects = f.Filter<Effect, Processed>();

            while (effects.NextUnsafe(
                       out EntityRef entity,
                       out Effect* _,
                       out _)) 
                f.Destroy(entity);
        }
    }
}