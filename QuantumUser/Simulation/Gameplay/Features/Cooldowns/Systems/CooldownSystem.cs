using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Cooldowns.Systems
{
    [Preserve]
    public unsafe class CooldownSystem : SystemMainThreadFilter<CooldownSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            CooldownLeft* cooldownable = f.Unsafe.GetPointer<CooldownLeft>(filter.Entity);
            cooldownable->Value -= f.DeltaTime;

            if (cooldownable->Value <= 0)
            {
                f.Add<CooldownUp>(filter.Entity);
                f.Remove<CooldownLeft>(filter.Entity);
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public Cooldown* Cooldown;
            public CooldownLeft* CooldownLeft;
        }
    }
}