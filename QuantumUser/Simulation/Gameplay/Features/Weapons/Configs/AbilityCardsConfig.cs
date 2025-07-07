using System.Collections.Generic;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs
{
	[CreateAssetMenu(menuName = "Cards", fileName = "AbilityCardConfig")]
	public class AbilityCardsConfig : ScriptableObject
	{
		public EAbilityCardId AbilityCardId;
		public List<AbilityCardLevel> Levels;
	}
}