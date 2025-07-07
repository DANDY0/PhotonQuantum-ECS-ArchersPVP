using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.StaticData
{
    public interface IStaticDataService
    {
        OrbitShotsSettingsConfig OrbitShotsSettings { get; }
        void LoadAll();
        WeaponConfig GetWeaponsConfig(EWeaponId abilityId);
        WeaponLevel GetWeaponLevel(EWeaponId abilityId, int level);
        AbilityCardsConfig GetAbilityCardsConfig(EAbilityCardId abilityCardId);
        AbilityCardLevel GetAbilityCardLevel(EAbilityCardId abilityCardId, int level);
    }
}