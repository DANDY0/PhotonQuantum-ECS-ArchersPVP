using Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.StatusVisuals;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Systems;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses
{
    namespace Quantum.QuantumUser.Simulation.Features.Movement
    {

        [Preserve]
        public unsafe class StatusFeature : SystemGroup
        {
            public StatusFeature() : base(nameof(StatusFeature), CreateSystems())
            {
            }

            private static SystemBase[] CreateSystems()
            {
                return new SystemBase[]
                {
                    new StatusDurationSystem(),
                    
                    new TicDamageStatusSystem(),
                    new ApplySlowSpeedStatusSystem(),

                    new StatusVisualsFeature(),
                    
                    new CleanupUnappliedStatusLinkedChanges(),
                    new CleanupUnappliedStatuses(),
                    
                };
            }
        }
    }
}