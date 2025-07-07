using System;
using System.Collections.Generic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs
{
	[Serializable]
	public class AbilityCardLevel
	{
		public List<EffectSetup> EffectSetups;
		public List<StatusSetup> StatusSetups;
		
		public ProjectileSetup ProjectileSetup;
		public AuraSetup AuraSetup;
	}
}