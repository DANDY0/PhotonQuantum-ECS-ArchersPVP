using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Cooldowns;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Systems
{
    [Preserve]
    public unsafe class BasicShotWeaponSystem : SystemMainThreadFilter<BasicShotWeaponSystem.Filter>
    {
        private IArmamentFactory _armamentFactory;
        private IStaticDataService _staticDataService;

        public override void OnInit(Frame f)
        {
            _armamentFactory = DI.Resolve<IArmamentFactory>();
            _staticDataService = DI.Resolve<IStaticDataService>();
        }

        public override void Update(Frame f, ref Filter filter)
        {
            var heroEntity = filter.Owner->Link.Entity;

            if (!f.Unsafe.TryGetPointer<PlayerLink>(heroEntity, out var heroLink) ||
                !f.Unsafe.TryGetPointer<PlayerActionState>(heroEntity, out var actionState) ||
                !f.Unsafe.TryGetPointer<CurrentTarget>(heroEntity, out var currentTarget) ||
                f.Unsafe.TryGetPointer<ObstaclePreventsAttack>(heroEntity, out var obstaclePreventsAttack) ||
                !f.Unsafe.TryGetPointer<Transform3D>(heroEntity, out var heroTransform) ||
                !f.Unsafe.TryGetPointer<WeaponsActive>(heroEntity, out var weaponsActive))
                return;

            if (actionState->Value != EPlayerActionState.Attacking)
                return;
            
            var targetEntity = currentTarget->Value.Entity;
            if (!f.Has<IsVisibleToEnemies>(targetEntity))
                return;
            
            if (!TryGetAttackDirection(f, targetEntity, heroTransform, out var attackDirection))
                return;

            ExecuteAttack(f, heroEntity, heroLink, heroTransform, attackDirection, filter.Entity);
        }

        private bool TryGetAttackDirection(Frame f, EntityRef targetEntity, Transform3D* heroTransform,
            out FPVector3 attackDirection)
        {
            attackDirection = FPVector3.Zero;

            if (!f.TryGet<Transform3D>(targetEntity, out var targetTransform))
                return false;

            FPVector3 direction = targetTransform.Position - heroTransform->Position;
            attackDirection = new FPVector3(direction.X, 0, direction.Z).Normalized;

            return true;
        }

        private void ExecuteAttack(Frame f, EntityRef heroEntity, PlayerLink* heroLink, Transform3D* heroTransform,
            FPVector3 attackDirection, EntityRef weaponEntity)
        {
            Debug.Log($"SHOT from {heroEntity}");

            var weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.BowShot, 1);

            _armamentFactory.CreatePendingShots(f, EWeaponId.BowShot, weaponEntity,
                f.Get<Owner>(heroLink->Entity), attackDirection);

            weaponEntity.PutOnCooldown(f, weaponLevel.Cooldown);

            f.Events.PlayerAttack(heroLink->Entity);

            //TODO add reveal time to some SO or playerData
            if (f.Has<InBush>(heroEntity)) 
                f.Set(heroEntity, new BushTemporaryReveal { Value = FP._3 });
        }

        public struct Filter
        {
            public EntityRef Entity;
            public BowShotWeapon* BowShotWeapon;
            public CooldownUp* CooldownUp;
            public Owner* Owner;
        }
    }
}
