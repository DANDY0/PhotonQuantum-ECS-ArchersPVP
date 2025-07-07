using Quantum.QuantumUser.Simulation.Gameplay.Features.Bushes.Systems;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Cooldowns.Systems;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Systems;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons
{
    public class WeaponFeature : SystemGroup
    {
        public WeaponFeature() : base(nameof(WeaponFeature), CreateSystems())
        {
            
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new CooldownSystem(),
                new BushRevealSystem(),
                
                new BasicShotWeaponSystem(),
                new OrbitingShotWeaponSystem(),
                new PendingShotSystem()
            };
        }
    }
}