using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;
using Quantum;
using Quantum.QuantumUser.Simulation.Gameplay.Levels;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class LevelInitializer : MonoBehaviour, IInitializable
    {
        public List<Transform> StartPoints;
        private ILevelDataProvider _levelDataProvider;

        public void Initialize()
        {
            List<FPVector3> fpPositions = StartPoints.Select(t => t.position.ToFPVector3()).ToList();
            _levelDataProvider.SetStartPoint(fpPositions);
        }

        [Inject]
        private void Construct(
            ILevelDataProvider levelDataProvider
        )
        {
            _levelDataProvider = levelDataProvider;
        }
    }
}