using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
    [Preserve]
    public unsafe class FinalizeHeroDeathProcessingSystem : SystemMainThreadFilter<FinalizeHeroDeathProcessingSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            f.Remove<ProcessingDeath>(filter.Hero);
        }

        public struct Filter
        {
            public EntityRef Hero;
            public PlayerLink* Link;
            public Dead* Dead;
            public ProcessingDeath* ProcessingDeath;
        }
    }
}