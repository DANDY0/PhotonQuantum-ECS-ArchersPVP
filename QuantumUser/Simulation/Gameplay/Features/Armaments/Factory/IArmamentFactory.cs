using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Factory
{
    public interface IArmamentFactory
    {
        EntityRef CreateBasicShot(Frame f, int level, EntityRef weapon, FPVector3 at, Owner owner, FPVector3 direction);
        EntityRef CreateBallShot(Frame f, int level, FPVector3 at, FP phase, Owner owner, EntityRef weapon);
        EntityRef CreatePendingShots(Frame f, EWeaponId weaponId, EntityRef weapon, Owner owner, FPVector3 attackDirection);
        EntityRef CreateBasicUltimateShot(Frame f, int level, EntityRef weapon, FPVector3 at, Owner owner, FPVector3 direction);
    }
}