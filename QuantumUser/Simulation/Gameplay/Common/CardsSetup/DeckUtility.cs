using Photon.Deterministic;
using Quantum.Collections;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;

namespace Quantum.QuantumUser.Simulation.Gameplay.Common.CardsSetup
{
	public class DeckUtility : IDeckUtility
	{
		private IStaticDataService _staticDataService;

		public void Init()
		{
			_staticDataService = DI.Resolve<IStaticDataService>();
		}

		public void AddCardAbilityToBowShotWeapon(Frame f, EntityRef weapon, EAbilityCardId abilityCardId)
		{
			QListPtr<StatusSetup> statusSetupsList;

			if (!f.TryGet(weapon, out StatusSetups existingStatusSetups))
			{
				statusSetupsList = f.AllocateList<StatusSetup>();
				f.Set(weapon, new StatusSetups { Value = statusSetupsList });
			}
			else
			{
				statusSetupsList = existingStatusSetups.Value;
			}

			var cardLevel = _staticDataService.GetAbilityCardLevel(abilityCardId, 1);
			f.AddToListComponent(statusSetupsList, cardLevel.StatusSetups);
		}
		
		public void AddCardAbilityToOrbitalShotWeapon(Frame f, EntityRef weapon, EAbilityCardId abilityCardId)
		{
			QListPtr<StatusSetup> statusSetupsList;

			if (!f.TryGet(weapon, out StatusSetups existingStatusSetups))
			{
				statusSetupsList = f.AllocateList<StatusSetup>();
				f.Set(weapon, new StatusSetups { Value = statusSetupsList });
			}
			else
			{
				statusSetupsList = existingStatusSetups.Value;
			}

			var cardLevel = _staticDataService.GetAbilityCardLevel(abilityCardId, 1);
			
			f.AddToListComponent(statusSetupsList, cardLevel.StatusSetups);
			
			f.Set(weapon, new OrbitLevel { Value = cardLevel.ProjectileSetup.OrbitLevel });

		}
	}
}