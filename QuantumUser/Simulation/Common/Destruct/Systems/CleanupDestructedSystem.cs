using Quantum.QuantumUser.Simulation.Gameplay.Features.Cleanup;
using Quantum.Task;

namespace Quantum.QuantumUser.Simulation.Common.Destruct.Systems
{
    public unsafe class CleanupDestructedSystem : SystemBase, ICleanupSystem
    {
        protected override TaskHandle Schedule(Frame f, TaskHandle taskHandle) => 
            taskHandle;

        public void Cleanup(Frame f)
        {
            ComponentFilter<Destructed> effects = f.Filter<Destructed>();

            while (effects.NextUnsafe(
                       out EntityRef entity,
                       out _)) 
                f.Destroy(entity);
        }
    }
}