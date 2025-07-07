using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using Quantum.QuantumUser.Simulation.Gameplay.Features.CharacterStats;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player
{
    [Preserve]
    public unsafe class PlayerSetupSystem : SystemSignalsOnly, ISignalSetupPlayer
    {
        public void SetupPlayer(Frame f, Owner owner, FPVector3 at, QBoolean isRespawn, QBoolean isDummy)
        {
            EntityRef entity = owner.Link.Entity;
            
            // Direction defaults
            f.Set(entity, new Direction { Value = FPVector3.Zero });
            f.Set(entity, new UltDirection { Value = FPVector3.Zero });

            // Modifiers
            var modifiersDict = f.AllocateDictionary<EStats, FP>();
            f.Set(entity, new StatsModifiers { Value = modifiersDict });

            // Get values from BaseStats 
            f.FillEnumDictionaryComponent(modifiersDict, InitStats.EmptyStatDictionary());
            var baseStats = f.ResolveDictionary(f.Get<BaseStats>(entity).Value);
            FP speed = baseStats[EStats.Speed];
            FP maxHp = baseStats[EStats.MaxHp];

            f.Set(entity, new Speed { Value = speed });
            //TODO base rotation add
            f.Set(entity, new RotationSpeed { Value = 5 });
            f.Set(entity, new CurrentHp { Value = maxHp });
            f.Set(entity, new MaxHp { Value = maxHp });


            f.Set(entity, new PlayerActionState { Value = EPlayerActionState.None });
            f.Set(entity, new PlayerLifeState { Value = EPlayerLifeState.Alive });

            if(!f.Has<UltimateId>(entity))
                f.Add(entity, new UltimateId { Value = EUltimateId.Basic });
            
            if (isDummy)
            {
                f.Unsafe.GetPointer<Transform3D>(entity)->Rotation = FPQuaternion.LookRotation(new FPVector3(0,0,-1), FPVector3.Up);
            }
            else
            {
                // f.Add<MovementAvailable>(entity);
                f.Add(entity, new AttackRange
                {
                    MinRange = FP._1_50,
                    MaxRange = FP._7,
                    CurrentRange = FP._2,
                    ExpansionRate = FP._4,
                    ContractionRate = FP._2
                });
                
                // LookDirection и Rotation
                FPVector3 dir = (FPVector3.Zero - at).Normalized;
                f.Set(entity, new LookDirection { Value = dir });
                f.Unsafe.GetPointer<Transform3D>(entity)->Rotation = FPQuaternion.LookRotation(dir, FPVector3.Up);
                f.Unsafe.GetPointer<Transform3D>(entity)->Position = at;
            }
            
            if (isRespawn)
            {
                if(f.Has<Dead>(entity))
                    f.Remove<Dead>(entity);
            }
            
            f.Events.PlayerSetupedEvent(f.Get<PlayerLink>(owner.Link.Entity), owner.TeamIndex);
        }
    }
}