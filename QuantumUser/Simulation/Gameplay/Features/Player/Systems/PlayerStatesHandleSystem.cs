using Photon.Deterministic;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Systems
{
   [Preserve]
   public unsafe class PlayerStatesHandleSystem : SystemMainThreadFilter<PlayerStatesHandleSystem.Filter>
   {
      public override void Update(Frame f, ref Filter filter)
      {
         Direction direction = f.Get<Direction>(filter.Entity);
         UltDirection ultDirection = f.Get<UltDirection>(filter.Entity);
         CurrentTarget currentTarget = default;
         
         if (f.Has<CurrentTarget>(filter.Entity))
            currentTarget = f.Get<CurrentTarget>(filter.Entity);

         bool canAttackTarget = f.Has<CurrentTarget>(filter.Entity) 
                                || f.Has<ObstaclePreventsAttack>(filter.Entity)
                                || !f.Has<InBush>(currentTarget.Value.Entity);
         
         bool isAttacking = f.Get<PlayerActionState>(filter.Entity).Value == EPlayerActionState.Attacking;
         
         if (f.Has<UltProcessing>(filter.Link->Entity))
         {
            if (f.Has<AttackPreparingDelay>(filter.Entity))
               f.Remove<AttackPreparingDelay>(filter.Entity);
            
            f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.UltProcessing });
         }
         else if (ultDirection.Value != FPVector3.Zero)
         {
            if (f.Has<AttackPreparingDelay>(filter.Entity))
               f.Remove<AttackPreparingDelay>(filter.Entity);
            
            f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.UltAiming });
         }
         else if (canAttackTarget && direction.Value == FPVector3.Zero)
         {
            if (!isAttacking)
               HandleAttackPreparing(f, ref filter);
            else
               f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.Attacking });
         }
         else if (direction.Value != FPVector3.Zero)
         {
            if (f.Has<AttackPreparingDelay>(filter.Entity))
               f.Remove<AttackPreparingDelay>(filter.Entity);

            f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.Moving });
         }
         else
         {
            if (f.Has<AttackPreparingDelay>(filter.Entity))
               f.Remove<AttackPreparingDelay>(filter.Entity);

            f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.Idle });
         }
      }

      private void HandleAttackPreparing(Frame f, ref Filter filter)
      {
         if (!f.Has<AttackPreparingDelay>(filter.Entity))
         {
            var baseStats = f.ResolveDictionary(filter.BaseStats->Value);
            f.Set(filter.Entity, new AttackPreparingDelay { Value = baseStats[EStats.AttackDelay] });
            f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.AttackPreparing });
         }
         else
         {
            AttackPreparingDelay* attackPreparing = f.Unsafe.GetPointer<AttackPreparingDelay>(filter.Entity);
            attackPreparing->Value -= f.DeltaTime;

            if (attackPreparing->Value <= FP._0)
            {
               f.Remove<AttackPreparingDelay>(filter.Entity);
               f.Set(filter.Entity, new PlayerActionState { Value = EPlayerActionState.Attacking });
            }
         }
      }

      public struct Filter
      {
         public EntityRef Entity;
         public PlayerLink* Link;
         public Owner* Owner;
         public BaseStats* BaseStats;
         public PlayerActionState* PlayerState;
      }
   }
}
