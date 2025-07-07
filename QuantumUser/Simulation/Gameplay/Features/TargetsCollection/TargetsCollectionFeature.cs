using Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection.Systems;
using UnityEngine.Scripting;

namespace Quantum.QuantumUser.Simulation.Gameplay.Features.TargetsCollection
{
    [Preserve]
    public class TargetsCollectionFeature : SystemGroup
    {
        public TargetsCollectionFeature() : base(nameof(TargetsCollectionFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new CollectTargetsIntervalSystems(),
                
                new CastForTargetsNoLimitSystem(),
                new CastForTargetsOncePerEntrySystem(),
                new CastForTargetsWithLimitSystem(),
                new MarkReachedSystem(),
                
                new CleanupTargetBuffersSystem(),
            };
        }
    }

}