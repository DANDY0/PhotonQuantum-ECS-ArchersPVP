using ModestTree.Util;
using Photon.Deterministic;
using Quantum.QuantumUser.Simulation.Common.Di;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.Movement.Systems
{
    [Preserve]
    public unsafe class OrbitalDeltaMoveSystem : SystemMainThread
    {
        private IStaticDataService _staticDataService;

        public override void OnInit(Frame f)
        {
            _staticDataService = DI.Resolve<IStaticDataService>();
        }

        public override void Update(Frame f)
        {
            var movers = f.Filter<
                OrbitPhase,
                OrbitCenterPosition,
                OrbitLevel,
                WorldPosition,
                Speed,
                MovementAvailable,
                Moving>();

            while (movers.NextUnsafe(
                       out EntityRef entity,
                       out OrbitPhase* orbitPhase,
                       out OrbitCenterPosition* orbitCenterPosition,
                       out OrbitLevel* orbitLevel,
                       out _,
                       out Speed* speed,
                       out _,
                       out _))
            {
                int direction = (int)orbitLevel->Value % 2 == 0 ? 1 : -1;

                var phase = orbitPhase->Value + direction * f.DeltaTime * speed->Value;
                f.Set(entity, new OrbitPhase { Value = phase });

                FP orbitRadiusLevel = _staticDataService.OrbitShotsSettings.GetRadius(orbitLevel->Value);

                FPVector3 newRelativePosition = new FPVector3(
                    FPMath.Cos(phase) * orbitRadiusLevel,
                    FP._0_50,
                    FPMath.Sin(phase) * orbitRadiusLevel);

                FPVector3 newPosition = newRelativePosition + orbitCenterPosition->Value;
                
                f.Set(entity, new WorldPosition { Value = newPosition });
            }
        }
    }
}
