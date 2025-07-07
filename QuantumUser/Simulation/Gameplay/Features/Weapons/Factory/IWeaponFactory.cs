namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Factory
{
    public interface IWeaponFactory
    {
        EntityRef CreateBowShotWeapon(Frame f, int level, Owner owner);
        EntityRef CreateOrbitingShotWeapon(Frame f, int level, Owner owner);
    }
}