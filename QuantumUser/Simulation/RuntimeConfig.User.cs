using Photon.Deterministic;
using Quantum.Assets;

namespace Quantum
{
    partial class RuntimeConfig
    {
        public AssetRef<GameSettingsData> GameSettingsData;
        
        // public AssetRef<EntityPrototype> BallPrototype;

        partial void SerializeUserData(BitStream stream)
        {
            stream.Serialize(ref GameSettingsData.Id);
            // stream.Serialize(ref BallPrototype.Id);
        }
    }
}