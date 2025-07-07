using Quantum.QuantumUser.Simulation.Common.Destruct.Systems;
using Quantum.QuantumUser.Simulation.Gameplay.Common.Time;
using Quantum.QuantumUser.Simulation.Gameplay.Features.ResetFeature.Systems;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.ResetFeature
{
    [Preserve]
    public class ResetFeature : SystemGroup
    {
        public ResetFeature() : base(nameof(ResetFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new RoundResetSystem(),
            };
        }
    }
}