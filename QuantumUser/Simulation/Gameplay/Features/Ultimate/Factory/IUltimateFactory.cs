using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Ultimate.Factory
{
    public interface IUltimateFactory
    {
        EntityRef CreateBasicUltimate(Frame f, int level, Owner owner, EUltimateId ultimateId, FPVector3 ultDirection);
    }
}