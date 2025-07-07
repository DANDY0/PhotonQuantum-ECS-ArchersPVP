using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Cooldowns
{
    public static class CooldownEntityExtensions
    {
        public static void PutOnCooldown(this EntityRef entity, Frame f)
        {
            if (!f.Has<Cooldown>(entity))
                return;

            f.Remove<CooldownUp>(entity);
            
            f.Set(entity, new CooldownLeft { Value = f.Get<Cooldown>(entity).Value });
        }

        public static void PutOnCooldown(this EntityRef entity, Frame f, FP cooldown)
        {
            f.Remove<CooldownUp>(entity);
            
            f.Set(entity, new Cooldown { Value = cooldown });
            f.Set(entity, new CooldownLeft { Value = cooldown });
        }
    }
}