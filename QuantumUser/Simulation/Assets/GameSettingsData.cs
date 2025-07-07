using Photon.Deterministic;
using UnityEngine.Serialization;

namespace Quantum.Assets
{
    public partial class GameSettingsData : AssetObject
    {
        public FP InitializationDuration = 3; //time to init quantum services
        public FP MatchIntroDuration = 3;
        public FP CountDownDuration = 3;
        public FP RoundDuration = 30;
        public FP GameDuration = 300;
        public FP RoundEndedIntervalDuration = 3;
        public FP GameOverDuration = 3;
        
        public int BestOfCountRounds = 1;
        
    }
}