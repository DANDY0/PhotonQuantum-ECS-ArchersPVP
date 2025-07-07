using Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets.Systems;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Player;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.AttackTargets
{
    public class AttackTargetsFeature : SystemGroup
    {
        public AttackTargetsFeature() : base(nameof(AttackTargetsFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                // new CurrentTargetSetSystem(), //uncomment, if need to search for nearest target
                new TargetSearchSystem(),
                new AttackRangeUpdateSystem(),
                new TargetVisibilitySystem(),
            };
        }
    }
}