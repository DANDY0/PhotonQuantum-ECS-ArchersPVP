using System;
using System.Collections.Generic;
using Photon.Deterministic;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs
{
    [CreateAssetMenu(menuName = "Weapons/OrbitShotsSettingsConfig", fileName = "OrbitShotsSettingsConfig")]
    public class OrbitShotsSettingsConfig : ScriptableObject
    {
        public List<OrbitRadiusSettingsData> OrbitRadiusSettingsData;

        private readonly Dictionary<EOrbitLevel, FP> _orbitRadiusDictionary = new();

        private void OnEnable()
        {
            foreach (OrbitRadiusSettingsData data in OrbitRadiusSettingsData) 
                _orbitRadiusDictionary.Add(data.OrbitRadiusLevel, data.OrbitRadius);
        }

        public FP GetRadius(EOrbitLevel level)
        {
            if (_orbitRadiusDictionary.TryGetValue(level, out FP value))
                return value;
            
            throw new Exception($"Orbit radius for level {level} doesn't exist");
        }
    }

    [Serializable]
    public class OrbitRadiusSettingsData
    {
        public EOrbitLevel OrbitRadiusLevel;
        public FP OrbitRadius;
    }
}