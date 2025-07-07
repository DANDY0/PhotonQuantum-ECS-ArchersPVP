using System.Collections.Generic;
using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Levels
{
    public class LevelDataProvider : ILevelDataProvider
    {
        public List<FPVector3> StartPoints { get; private set; }

        public void SetStartPoint(List<FPVector3> startPoint)
        {
            StartPoints = startPoint;
        }
    }
}