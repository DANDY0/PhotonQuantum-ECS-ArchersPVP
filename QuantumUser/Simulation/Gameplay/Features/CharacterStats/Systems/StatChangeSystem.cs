using System.Collections.Generic;
using Photon.Deterministic;
using Quantum.Collections;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats.Systems
{
    [Preserve]
    public unsafe class StatChangeSystem : SystemMainThreadFilter<StatChangeSystem.Filter>
    {
        public override void Update(Frame f, ref Filter filter)
        {
            QEnumDictionary<EStats, FP> baseStats = f.ResolveDictionary(f.Unsafe.GetPointer<BaseStats>(filter.Entity)->Value);
            QEnumDictionary<EStats, FP> statModifiers = f.ResolveDictionary(f.Unsafe.GetPointer<StatsModifiers>(filter.Entity)->Value);

            foreach (KeyValuePair<EStats, FP> stat in baseStats)
            {
                statModifiers[stat.Key] = FP._0;

                foreach (EntityComponentPair<StatChange> statChange in f.GetComponentIterator<StatChange>())
                {
                    if (IsMatchingStatChange(f, filter, statChange, stat))
                    {
                        var value = f.Get<EffectValue>(statChange.Entity).Value;
                        statModifiers[stat.Key] += value;
                        // Debug.Log($"StatChange {stat.Key}: {value}");
                    }
                }
            }
        }

        private bool IsMatchingStatChange(Frame f, Filter filter, EntityComponentPair<StatChange> statChange, KeyValuePair<EStats, FP> stat) => 
            f.Get<TargetId>(statChange.Entity).Value == filter.Entity && statChange.Component.Value == stat.Key;

        public struct Filter
        {
            public EntityRef Entity;
            public BaseStats* BaseStats;
            public StatsModifiers* StatModifiers;
        }
    }
}