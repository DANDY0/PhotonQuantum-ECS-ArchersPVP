using Quantum.QuantumUser.Simulation.Common.Destruct.Systems;

namespace Quantum.QuantumUser.Simulation.Common.Destruct
{
    public class ProcessDestructedFeature : SystemGroup
    {
        public ProcessDestructedFeature() : base(nameof(ProcessDestructedFeature), CreateSystems())
        {
            
        }
        
        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new SelfDestructTimerSystem(),
                
                new CleanupDestructedSystem(),
            };
        }

    }
}