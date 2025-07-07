namespace Quantum.Gameplay.Features.Match
{
    public class MatchFeature: SystemGroup
    {
        public MatchFeature() : base(nameof(MatchFeature), CreateSystems())
        {
        }

        private static SystemBase[] CreateSystems()
        {
            return new SystemBase[]
            {
                new RoundEndedTriggerSystem(),
                new GameOverTriggerSystem(),
            };
        }
    }
}