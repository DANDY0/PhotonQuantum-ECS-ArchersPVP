using System;
using System.Collections.Generic;
using System.Linq;
using Photon.Deterministic;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats
{
    public static class InitStats
    {
        public static Dictionary<EStats, FP> EmptyStatDictionary()
        {
            return Enum.GetValues(typeof(EStats))
                .Cast<EStats>()
                .Except(new[] { EStats.Unknown })
                .ToDictionary(x => x, _ => FP._0);
        } 
    }
}