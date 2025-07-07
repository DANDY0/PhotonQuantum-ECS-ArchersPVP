using System.Collections.Generic;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs
{
    [CreateAssetMenu(menuName = "Weapons", fileName = "WeaponConfig")]
    public class WeaponConfig : ScriptableObject
    {
        public EWeaponId WeaponId;
        public List<WeaponLevel> Levels;
    }
}