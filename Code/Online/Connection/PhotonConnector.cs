using System;
using System.Threading;
using System.Threading.Tasks;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Photon.Client;
using Photon.Realtime;
using Quantum;
using UnityEngine;
using Zenject;

namespace Code.Connection
{
    public class PhotonConnector
    {
        private readonly PhotonClientArgsProvider _photonClientArgsProvider;
        private AppSettings _appSettings;
        private RealtimeClient _client;
        private CancellationTokenSource _cancellation;

        public PhotonConnector(PhotonClientArgsProvider photonClientArgsProvider)
        {
            _photonClientArgsProvider = photonClientArgsProvider;
            _appSettings = new AppSettings(PhotonServerSettings.Global.AppSettings);
        }

        public async Task Connect()
        {
            _client = new RealtimeClient(_appSettings.Protocol);
            
            _photonClientArgsProvider.Client = _client; //TEMPPPPP
            
            _cancellation = new CancellationTokenSource();
            Debug.Log("[PhotonConnector] Starting connection...");

            try
            {
                var config = new AsyncConfig
                {
                    CancellationToken = _cancellation.Token,
                    TaskFactory = AsyncConfig.CreateUnityTaskFactory()
                };

                await _client.ConnectUsingSettingsAsync(_appSettings, config);

                Debug.Log($"[PhotonConnector] Connected to Master in region: {_client.CloudRegion}");
            }
            catch (Exception ex)
            {
                Debug.LogError($"[PhotonConnector] Connection failed: {ex.Message}");
            }
        }

        private void OnDestroy()
        {
            _cancellation?.Cancel();
            _cancellation?.Dispose();
        }
    }
}