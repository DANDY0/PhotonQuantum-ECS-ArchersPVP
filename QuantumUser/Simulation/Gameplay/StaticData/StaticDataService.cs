using System;
using System.Collections.Generic;
using System.Linq;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.StaticData
{
    public class StaticDataService : IStaticDataService
    {
        private Dictionary<EWeaponId, WeaponConfig> _weaponById;
        private Dictionary<EAbilityCardId, AbilityCardsConfig> _abilityCardById;

        public OrbitShotsSettingsConfig OrbitShotsSettings { get; private set; }

        public void LoadAll()
        {
            LoadWeapons();
            LoadCards();
            LoadOrbitShotsSettingsConfig();
        }
        
        public WeaponConfig GetWeaponsConfig(EWeaponId weaponId)
        {
            if (_weaponById.TryGetValue(weaponId, out var config))
                return config;

            throw new Exception($"weapon config for {weaponId} was not found");
        }

        public WeaponLevel GetWeaponLevel(EWeaponId weaponId, int level)
        {
            var config = GetWeaponsConfig(weaponId);

            if (level > config.Levels.Count)
                level = config.Levels.Count;

            return config.Levels[level - 1];
        }
        
        public AbilityCardsConfig GetAbilityCardsConfig(EAbilityCardId abilityCardId)
        {
            if (_abilityCardById.TryGetValue(abilityCardId, out var config))
                return config;

            throw new Exception($"AbilityCard config for {abilityCardId} was not found");
        }

        public AbilityCardLevel GetAbilityCardLevel(EAbilityCardId abilityCardId, int level)
        {
            var config = GetAbilityCardsConfig(abilityCardId);

            if (level > config.Levels.Count)
                level = config.Levels.Count;

            return config.Levels[level - 1];
        }
        
        private void LoadWeapons()
        {
            _weaponById = Resources
                .LoadAll<WeaponConfig>("Configs/Weapons")
                .ToDictionary(x => x.WeaponId, x => x);
        }

        private void LoadCards()
        {
            _abilityCardById = Resources
                .LoadAll<AbilityCardsConfig>("Configs/AbilityCards")
                .ToDictionary(x => x.AbilityCardId, x => x);
        }

        private void LoadOrbitShotsSettingsConfig()
        {
            OrbitShotsSettings = Resources.Load<OrbitShotsSettingsConfig>("Configs/OrbitShotsSettingsConfig");
        }
    }
}