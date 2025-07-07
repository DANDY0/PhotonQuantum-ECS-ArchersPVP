using System;
using Photon.Realtime;
using Quantum;
using Quantum.Menu;
using UnityEngine;

namespace Code.Connection
{
    //TODO TEMPPPPPPP
    public class PhotonClientArgsProvider: MonoBehaviour, IPhotonConnectionService
    {
        public event Action<RealtimeClient> RealtimeClientSetEvent;
        
        public RealtimeClient Client
        {
            get => _client;
            set
            {
                _client = value;
                RealtimeClientSetEvent?.Invoke(_client);
            }
        }

        public QuantumMenuConnectArgs ConnectArgs => _connectionArgs;
        public RuntimePlayer RuntimeLocalPlayer => _connectionArgs.RuntimePlayers[0];
        
        [SerializeField] private QuantumMenuConfig _config;
        [SerializeField] private QuantumMenuConnectArgs _connectionArgs;
        
        private RealtimeClient _client;

        protected void Awake()
        {
            _config.Init();
            
            _connectionArgs.LoadFromPlayerPrefs();
            _connectionArgs.SetDefaults(_config);
            _connectionArgs.RuntimeConfig = JsonUtility.FromJson<RuntimeConfig>(JsonUtility.ToJson(_connectionArgs.Scene.RuntimeConfig)); 
            
            DontDestroyOnLoad(gameObject);
        }
    }
}