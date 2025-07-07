namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Factory
{
    public interface IStatusFactory
    {
        EntityRef CreateStatus(Frame f, StatusSetup setup, EntityRef producerRef, EntityRef targetRef, Owner owner);
    }
}