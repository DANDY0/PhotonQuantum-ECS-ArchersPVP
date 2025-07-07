namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Factory
{
    public interface IEffectFactory
    {
        EntityRef CreateEffect(Frame f, EffectSetup setup, EntityRef producerId, EntityRef targetId, Owner owner);
    }
}