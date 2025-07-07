using Quantum.QuantumUser.Simulation.Gameplay.Common.Time;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Bushes.Systems;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player
{
    [Preserve]
    public class PlayerFeature : SystemGroup
    {
        public PlayerFeature() : base(nameof(PlayerFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new PlayerCreationSystem(),
                new PlayerSetupSystem(),
                new PlayerMovementAvailableTriggeredSystem(),
                new PlayerDirectionSetSystem(),
                new PlayerUltDirectionSetSystem(),
                new PlayerSetLookRotationByAttackSystem(),
                new PlayerBushEnterExitSystem(),
                new PlayerVisibleToEnemies(),
                new PlayerStatesHandleSystem(),
                new PlayerMovementSystem(),
                new PlayerDeathSystem(),
                new AnimationStatesEventsSystem(),
                new FinalizeHeroDeathProcessingSystem(),
                new TimeSystem(),
            };
        }
    }
}
