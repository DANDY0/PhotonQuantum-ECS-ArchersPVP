using ModestTree.Util;
using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Cooldowns;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Ultimate.Systems
{
    [Preserve]
    public unsafe class BasicUltimateSystem : SystemMainThreadFilter<BasicUltimateSystem.Filter>
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
            var heroes = f.Filter<PlayerLink, UltProcessing, Transform3D>();

            while (heroes.NextUnsafe(
                       out EntityRef heroEntity,
                       out PlayerLink* heroLink,
                       out _,
                       out Transform3D* heroPosition))
            {
                EntityRef ultimateEntity = FindUltimateForHero(filter, heroEntity);
                if (ultimateEntity == EntityRef.None)
                    continue;


                ExecuteAttack(f, heroEntity, heroLink, heroPosition, filter.UltDirection->Value.Normalized, ultimateEntity);
                f.Remove<UltProcessing>(heroEntity);
            }

            f.Destroy(filter.Entity);
        }

        private EntityRef FindUltimateForHero(Filter filter, EntityRef heroEntity) =>
            filter.Owner->Link.Entity == heroEntity ? filter.Entity : EntityRef.None;

        private void ExecuteAttack(Frame f, EntityRef heroEntity, PlayerLink* heroLink, Transform3D* heroPosition,
            FPVector3 attackDirection, EntityRef weaponEntity)
        {
            Debug.Log($"SHOT from {heroEntity}");

            ProjectileSetup projectileSetup = _staticDataService.GetWeaponLevel(EWeaponId.BowShot, 1).ProjectileSetup;
            FPVector3 playerPosition = heroPosition->Position;
            
            FPVector3 shotAt = playerPosition + attackDirection * projectileSetup.MuzzleDistanceXZ;
            FPVector3 positionOffset = new FPVector3(FP._0, projectileSetup.MuzzleDistanceY, FP._0);

            EntityRef shotEntity = _armamentFactory.CreateBasicShot(f, 1,
                weaponEntity,
                shotAt + positionOffset,
                f.Get<Owner>(heroLink->Entity),
                attackDirection);

            f.Set(shotEntity, new ProducerId { Value = heroEntity });
            f.Set(shotEntity, new Direction { Value = attackDirection });
            f.Add<Moving>(shotEntity);

            weaponEntity.PutOnCooldown(f, _staticDataService.GetWeaponLevel(EWeaponId.BowShot, 1).Cooldown);

            f.Events.PlayerAttack(heroLink->Entity);
        }
        

        public struct Filter
        {
            public EntityRef Entity;
            public BasicUltimate* BasicUltimate;
            public UltDirection* UltDirection;
            public Owner* Owner;
        }
    }
}