namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Applier
{
    public interface IStatusApplier
    { 
        EntityRef ApplyStatus(Frame frame, StatusSetup setup, EntityRef producer, EntityRef target);
    }
}