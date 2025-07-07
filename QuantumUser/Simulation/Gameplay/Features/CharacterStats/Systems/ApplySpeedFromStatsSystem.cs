using Photon.Deterministic;
using Quantum.Collections;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats.Systems
{
    [Preserve]
    public unsafe class ApplySpeedFromStatsSystem : SystemMainThreadFilter<ApplySpeedFromStatsSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            FP moveSpeed = MoveSpeed(f, filter).ZeroIfNegative();

            f.Set(filter.StatOwner, new Speed { Value = moveSpeed });
            // Debug.Log($"ApplySpeed : {moveSpeed}");
        }

        private FP MoveSpeed(Frame f, Filter filter)
        {
            QEnumDictionary<EStats, FP> baseStats = f.ResolveDictionary(f.Unsafe.GetPointer<BaseStats>(filter.StatOwner)->Value);
            QEnumDictionary<EStats, FP> statsModifiers = f.ResolveDictionary(f.Unsafe.GetPointer<StatsModifiers>(filter.StatOwner)->Value);

            FP baseSpeed = baseStats[EStats.Speed];
            FP modifierSpeed = statsModifiers[EStats.Speed];

            return baseSpeed - modifierSpeed;
        }


        public struct Filter
        {
            public EntityRef StatOwner;
            public BaseStats* BaseStats;
            public StatsModifiers* StatsModifiers;
            public Speed* Speed;
        }
    }
}