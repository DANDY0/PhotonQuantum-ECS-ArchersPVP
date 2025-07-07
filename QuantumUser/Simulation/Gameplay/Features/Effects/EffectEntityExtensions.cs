namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Effects
{
    public static class EffectEntityExtensions
    {
        public static EntityRef? Producer(this EntityRef effect, Frame f)
        {
            return f.Has<ProducerId>(effect)
                ? f.Get<ProducerId>(effect).Value
                : null;
        }

        public static EntityRef? Target(this EntityRef effect, Frame f)
        {
            return f.Has<TargetId>(effect)
                ? f.Get<TargetId>(effect).Value
                : null;
        }
    }
}