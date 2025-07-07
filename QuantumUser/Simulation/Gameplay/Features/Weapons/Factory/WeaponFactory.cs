using Quantum.QuantumUser.Simulation.Common.Extensions;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Factory
{
    public class WeaponFactory : IWeaponFactory
    {
        private readonly IStaticDataService _staticDataService;

        public WeaponFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public EntityRef CreateBowShotWeapon(Frame f, int level, Owner owner)
        {
            WeaponLevel weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.BowShot, level);

            var entityRef = f.CreateEmpty(owner);
            f.Set(entityRef, new WeaponId { Value = EWeaponId.BowShot });
            f.Set(entityRef, new ProducerId { Value = owner.Link.Entity });
            f.Add<BowShotWeapon>(entityRef);
            f.Set(entityRef, new Cooldown { Value = weaponLevel.Cooldown });
            f.Set(entityRef, new CooldownUp());
            
            return entityRef;
        }
        
        public EntityRef CreateOrbitingShotWeapon(Frame f, int level, Owner owner)
        {
            WeaponLevel weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.OrbitalShot, level);

            var entityRef = f.CreateEmpty(owner);
            f.Set(entityRef, new WeaponId { Value = EWeaponId.OrbitalShot });
            f.Set(entityRef, new ProducerId { Value = owner.Link.Entity });
            f.Add<OrbitalShotWeapon>(entityRef);
            f.Set(entityRef, new Cooldown { Value = weaponLevel.Cooldown });
            f.Set(entityRef, new CooldownUp());
            
            return entityRef;
        }
    }
}