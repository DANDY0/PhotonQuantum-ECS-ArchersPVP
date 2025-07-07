using System;
using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs
{
    [Serializable]
    public class WeaponLevel
    {
        public FP Cooldown;

        public AssetRef<EntityPrototype> ViewPrefab;
        
        public List<EffectSetup> EffectSetups;
        public List<StatusSetup> StatusSetups;
        public List<ETeamRelation> Hit;
        
        public ProjectileSetup ProjectileSetup;
        public AuraSetup AuraSetup;
    }
}