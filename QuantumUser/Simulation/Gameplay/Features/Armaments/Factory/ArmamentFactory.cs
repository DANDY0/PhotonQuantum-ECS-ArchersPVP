using System.Linq;
using Photon.Deterministic;
using Quantum.Collections;
using Quantum.QuantumUser.Simulation.Common.Extensions;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Factory
{
    [Preserve]
    public unsafe class ArmamentFactory : IArmamentFactory
    {
        private readonly IStaticDataService _staticDataService;

        public ArmamentFactory(IStaticDataService staticDataService)
        {
            _staticDataService = staticDataService;
        }
        
        public EntityRef CreateBasicShot(Frame f, int level, EntityRef weapon, FPVector3 at, Owner owner, FPVector3 direction)
        {
            WeaponLevel weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.BowShot, level);
            var projectile = CreateProjectile(f, weaponLevel, weapon, at, owner, direction);

            SendShotCreatedEvent(f, owner);

            return projectile;
        }

        public EntityRef CreateBallShot(Frame f, int level, FPVector3 at, FP phase, Owner owner, EntityRef weapon)
        {
            WeaponLevel weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.OrbitalShot, level);
            EOrbitLevel orbitRadius = f.Get<OrbitLevel>(weapon).Value;

            EntityRef ballShotEntity = CreateProjectile(f, weaponLevel, weapon, at, owner, new FPVector3());

            f.Set(ballShotEntity, new OrbitPhase { Value = phase });
            f.Set(ballShotEntity, new OrbitLevel { Value = orbitRadius });

            return ballShotEntity;
        }
        
        public EntityRef CreateBasicUltimateShot(Frame f, int level, EntityRef weapon, FPVector3 at, Owner owner, FPVector3 direction)
        {
            WeaponLevel weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.BowShot, level);
            var projectile = CreateProjectile(f, weaponLevel, weapon, at, owner, direction);

            SendShotCreatedEvent(f, owner);

            return projectile;
        }

        private static void SendShotCreatedEvent(Frame f, Owner owner)
        {
            StatusSetups statuses = new StatusSetups();
            if(f.Has<StatusSetups>(owner.Link.Entity))
                statuses = f.Get<StatusSetups>(owner.Link.Entity);

            f.Events.BasicShotCreated(owner.Link.Entity, statuses.Value);
        }

        private EntityRef CreateProjectile(Frame f, WeaponLevel weaponLevel, EntityRef weapon, FPVector3 at,
            Owner owner, FPVector3 direction)        
        {
            ProjectileSetup setup = weaponLevel.ProjectileSetup;
            EntityRef armamentEntity = f.CreateEmpty(weaponLevel.ViewPrefab, owner);

            f.Add<Armament>(armamentEntity);
            f.Add<MovementAvailable>(armamentEntity);
            f.Add<ReadyToCollectTargets>(armamentEntity);
            f.Add<CollectingTargetsContinuously>(armamentEntity);
            
            f.Set(armamentEntity, new Speed { Value = setup.Speed });
            f.Set(armamentEntity, new WorldPosition { Value = at });
            f.Set(armamentEntity, new SelfDestructTimer { Value = setup.Lifetime });
            f.Set(armamentEntity, new Radius { Value = setup.ContactRadius });
            f.Set(armamentEntity, new TargetLayerMask { Value = CollisionLayer.Hero.AsMask() });
            
            //set rotation to target
            FPQuaternion targetRotation = FPQuaternion.LookRotation(direction, FPVector3.Up);
            Transform3D* transform = f.Unsafe.GetPointer<Transform3D>(armamentEntity);
            transform->Rotation = targetRotation;
            
            if (!weaponLevel.EffectSetups.IsNullOrEmpty())
            {
                QListPtr<EffectSetup> effectSetupsList = f.AllocateList<EffectSetup>();
                f.Set(armamentEntity, new EffectSetups { Value = effectSetupsList });
                f.FillListComponent(effectSetupsList, weaponLevel.EffectSetups);
            }
            
            //add weaponStatuses
            if (!weaponLevel.StatusSetups.IsNullOrEmpty())
            {
                QListPtr<StatusSetup> weaponStatus = f.AllocateList<StatusSetup>();
                f.Set(armamentEntity, new StatusSetups { Value = weaponStatus });
                f.FillListComponent(weaponStatus, weaponLevel.StatusSetups);
            }
            
            //get or create list to modify
            QListPtr<StatusSetup> statusSetupsList;
            if (f.TryGet(armamentEntity, out StatusSetups existingStatusSetups))
                statusSetupsList = existingStatusSetups.Value;
            else
            {
                statusSetupsList = f.AllocateList<StatusSetup>(); 
                f.Set(armamentEntity, new StatusSetups { Value = statusSetupsList });
            }
            
            //add abilityStatuses
            if (f.Has<StatusSetups>(weapon))
            {
                StatusSetups* statusSetups = f.Unsafe.GetPointer<StatusSetups>(weapon);  
                f.AddToListComponent(statusSetupsList, f.ResolveList(statusSetups->Value).ToList());
            }
            
            if (!weaponLevel.Hit.IsNullOrEmpty())
            {
                QListPtr<ETeamRelation> targetRelations = f.AllocateList<ETeamRelation>();
                f.Set(armamentEntity, new TargetRelations { Value = targetRelations });
                f.FillListComponent(targetRelations, weaponLevel.Hit);
            }

            if (setup.Pierce > 0)
                f.Set(armamentEntity, new TargetLimit { Value = setup.Pierce });

            f.Set(armamentEntity, new TargetBuffer { Value = f.AllocateList<EntityRef>() });
            f.Set(armamentEntity, new ProcessedTargets { Value = f.AllocateList<EntityRef>() });
            
            f.Unsafe.GetPointer<Transform3D>(armamentEntity)->Position = at;
            
            return armamentEntity;
        }

        public EntityRef CreatePendingShots(Frame f, EWeaponId weaponId, EntityRef weapon, Owner owner, FPVector3 attackDirection)
        {
            WeaponLevel weaponLevel = _staticDataService.GetWeaponLevel(EWeaponId.BowShot, 1);
            
            EntityRef pendingShot = f.CreateEmpty(owner);
            f.Add(pendingShot, new ProjectilesCount { Value = weaponLevel.ProjectileSetup.ProjectileCount });
            f.Add(pendingShot, new PendingShotsCount { Value = weaponLevel.ProjectileSetup.PendingShotsCount });
            f.Add(pendingShot, new PendingShotInterval { Value = weaponLevel.ProjectileSetup.PendingShotInterval });
            f.Add(pendingShot, new TimeSinceLastTick { Value = weaponLevel.ProjectileSetup.PendingShotInterval });
            f.Add(pendingShot, new WeaponRef { Value = weapon });
            f.Set(pendingShot, owner);
            f.Add(pendingShot, new Direction { Value = attackDirection });

            return pendingShot;
        }
    }
}
