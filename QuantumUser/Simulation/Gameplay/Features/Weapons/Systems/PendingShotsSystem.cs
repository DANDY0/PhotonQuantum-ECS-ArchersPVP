using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Configs;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Systems
{
    [Preserve]
    public unsafe class PendingShotSystem : SystemMainThreadFilter<PendingShotSystem.Filter>
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
            TimeSinceLastTick* pendingShotTimer = f.Unsafe.GetPointer<TimeSinceLastTick>(filter.PendingShotsEntity);
            pendingShotTimer->Value -= f.DeltaTime;
            if (pendingShotTimer->Value <= FP._0)
            {
                var weaponRef = f.Unsafe.GetPointer<WeaponRef>(filter.PendingShotsEntity);
                PendingShotsCount* pendingShotsCount = f.Unsafe.GetPointer<PendingShotsCount>(filter.PendingShotsEntity);

                if (pendingShotsCount->Value > 0)
                {
                    ProjectileSetup projectileSetup = _staticDataService.GetWeaponLevel(EWeaponId.BowShot, 1).ProjectileSetup;
                    int projectileCount = projectileSetup.ProjectileCount;

                    FPVector3 playerPosition = f.Get<Transform3D>(filter.Owner->Link.Entity).Position;
                    FPVector3 direction = filter.Direction->Value;

                    FPVector3 shotAt = playerPosition + direction.Normalized * projectileSetup.MuzzleDistanceXZ;

                    for (int i = 0; i < projectileCount; i++)
                    {
                        FPVector3 positionOffset = CalculateShotOffset(i, projectileCount, direction);
                        positionOffset.Y = projectileSetup.MuzzleDistanceY;
                        
                        EntityRef shotEntity = _armamentFactory.CreateBasicShot(f, 1,
                            weaponRef->Value,
                            shotAt + positionOffset,
                            f.Get<Owner>(filter.PendingShotsEntity),
                            direction);

                        f.Set(shotEntity, new ProducerId { Value = filter.Owner->Link.Entity });
                        f.Set(shotEntity, new Direction { Value = direction });
                        f.Add<Moving>(shotEntity);
                    }

                    pendingShotsCount->Value -= 1;

                    if (pendingShotsCount->Value <= 0)
                        f.Add<Destructed>(filter.PendingShotsEntity);
                    else
                        pendingShotTimer->Value = filter.PendingShotInterval->Value;
                }
            }
        }

        private FPVector3 CalculateShotOffset(int index, int totalShots, FPVector3 direction)
        {
            FP spacing = FP._0_50;

            FPVector3 perpendicular = new FPVector3(-direction.Z, FP._0, direction.X).Normalized;

            FP offset = (FP)index - (FP)(totalShots - 1) / FP._2;
            return perpendicular * offset * spacing;
        }

        
        public struct Filter
        {
            public EntityRef PendingShotsEntity;
            public PendingShotsCount* PendingShotsCount;
            public PendingShotInterval* PendingShotInterval;
            public WeaponRef* WeaponRef;
            public Owner* Owner;
            public Direction* Direction;
        }
    }
}
