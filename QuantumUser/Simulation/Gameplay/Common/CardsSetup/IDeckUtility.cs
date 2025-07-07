namespace Quantum.QuantumUser.Simulation.Gameplay.Common.CardsSetup
{
	public interface IDeckUtility
	{
		public void Init();

		public void AddCardAbilityToBowShotWeapon(Frame f, EntityRef weapon, EAbilityCardId abilityCardId);
		public void AddCardAbilityToOrbitalShotWeapon(Frame f, EntityRef weapon, EAbilityCardId abilityCardId);
	}
}