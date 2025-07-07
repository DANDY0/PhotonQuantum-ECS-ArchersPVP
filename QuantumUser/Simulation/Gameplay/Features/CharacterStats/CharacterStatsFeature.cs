using Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats.Systems;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats
{
    [Preserve]
    public class CharacterStatsFeature : SystemGroup
    {
        public CharacterStatsFeature() : base(nameof(CharacterStatsFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new StatChangeSystem(),
                
                new ApplySpeedFromStatsSystem(),
            };
        }
    }

}