using System.Collections.Generic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Cleanup
{
    public class CleanupFeature : SystemMainThread
    {
        private readonly List<ICleanupSystem> _cleanupSystems = new();

        public CleanupFeature(ICollection<SystemBase> systems)
        {
            foreach (var system in systems) 
                ProcessSystem(system);
        }

        private void ProcessSystem(SystemBase system)
        {
            if (system is ICleanupSystem cleanupSystem) 
                _cleanupSystems.Add(cleanupSystem);

            foreach (SystemBase childSystem in system.ChildSystems) 
                ProcessSystem(childSystem);
        }

        public override void Update(Frame f)
        {
            foreach (ICleanupSystem system in _cleanupSystems) 
                system.Cleanup(f);
        }
    }
}