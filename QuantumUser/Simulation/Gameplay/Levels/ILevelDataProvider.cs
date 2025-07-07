using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Levels
{
    public interface ILevelDataProvider
    {
        List<FPVector3> StartPoints { get; }
        void SetStartPoint(List<FPVector3> startPoint);
    }
}