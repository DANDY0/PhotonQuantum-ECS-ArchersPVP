using Photon.Realtime;
using Quantum.Menu;

namespace Code.Connection
{
    public interface IPhotonConnectionService
    {
        RealtimeClient Client { get; set; }
        QuantumMenuConnectArgs ConnectArgs { get; }
    }
}