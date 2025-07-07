using System;

namespace Quantum.QuantumUser.Simulation.Gameplay.Common.Time
{
    public interface ITimeService
    {
        float DeltaTime { get; }
        DateTime UtcNow { get; }
        void StopTime();
        void StartTime();
    }
}