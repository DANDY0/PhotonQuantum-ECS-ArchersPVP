using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Common.Destruct.Systems
{
    [Preserve]
    public unsafe class SelfDestructTimerSystem : SystemMainThreadFilter<SelfDestructTimerSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            if (f.Unsafe.GetPointer<SelfDestructTimer>(filter.Entity)->Value > 0)
                f.Unsafe.GetPointer<SelfDestructTimer>(filter.Entity)->Value -= f.DeltaTime;
            else
            {
                f.Remove<SelfDestructTimer>(filter.Entity);
                f.Destroy(filter.Entity);
            }
        }

        public struct Filter
        {
            public EntityRef Entity;
            public SelfDestructTimer* Timer;
        }
    }
}