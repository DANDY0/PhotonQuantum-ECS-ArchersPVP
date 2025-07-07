using UnityEngine.Scripting;

namespace Quantum.Gameplay.Features.GameplayStates
{
  
    [Preserve]
    public class GameplayStatesFeature : SystemGroup
    {
        public GameplayStatesFeature() : base(nameof(GameplayStatesFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new GameSystem(),
            };
        }
    }
}