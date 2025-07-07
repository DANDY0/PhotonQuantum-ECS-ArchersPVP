using Code.Connection;
using Code.Connection.Code.Connection;
using Code.Debuging;
using Code.Gameplay.StaticData;
using Code.Infrastructure.Loading;
using Code.Infrastructure.States.Factory;
using Code.Infrastructure.States.GameStates;
using Code.Infrastructure.States.StateMachine;
using Code.Menu;
using Code.Online.Data;
using Code.Online.Gameplay;
using Code.Online.Matchmaking;
using Code.UI.Gameplay;
using Code.UI.Gameplay.Intro;
using Code.UI.Menu.Windows.Menu;
using Code.UI.Windows;
using Quantum.QuantumUser.Simulation.Gameplay.Common.CardsSetup;
using Quantum.QuantumUser.Simulation.Gameplay.Common.Time;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Armaments.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Effects.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Player.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Applier;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Statuses.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Ultimate.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Features.Weapons.Factory;
using Quantum.QuantumUser.Simulation.Gameplay.Levels;
using Quantum.QuantumUser.Simulation.Gameplay.StaticData;
using RSG;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class BootstrapInstaller : MonoInstaller, ICoroutineRunner, IInitializable
    {
        [SerializeField] private PhotonClientArgsProvider _photonClientArgsProvider;
        
        public void Initialize()
        {
            Promise.UnhandledException += LogPromiseException;

            Container.Resolve<IGameStateMachine>().Enter<BootstrapState>();
        }

        private void LogPromiseException(object sender, ExceptionEventArgs e)
        {
            Debug.LogError(e.Exception);
        }

        public override void InstallBindings()
        {
            BindInfrastructureServices();
            BindConnectionServices();
            BindMatchmakingHandlers();
            
            BindCommonServices();
            BindGameplayServices();
            
            BindGameplayFactories();
            
            BindStateMachine();
            BindStateFactory();
            BindGameStates();
            BindDataServices();
        }

        private void BindDataServices()
        { 
            Container.Bind<MatchSyncDataService>().AsSingle();
        }

        private void BindGameplayServices()
        {
            Container.Bind<IUnityStaticDataService>().To<UnityStaticDataService>().AsSingle();
            Container.Bind<IStaticDataService>().To<StaticDataService>().AsSingle();
            Container.Bind<IWindowService>().To<WindowService>().AsSingle();
            Container.Bind<ILevelDataProvider>().To<LevelDataProvider>().AsSingle();
            Container.Bind<IStatusApplier>().To<StatusApplier>().AsSingle();
            Container.Bind<IDeckUtility>().To<DeckUtility>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<MatchmakingService>().AsSingle();
            Container.BindInterfacesAndSelfTo<GameplayDisconnectService>().AsSingle();
            
            Container.BindInterfacesAndSelfTo<DebugToolsService>().AsSingle();
        }

        private void BindGameplayFactories()
        {
            Container.Bind<IPlayerFactory>().To<PlayerFactory>().AsSingle();
            Container.Bind<IArmamentFactory>().To<ArmamentFactory>().AsSingle();
            Container.Bind<IWeaponFactory>().To<WeaponFactory>().AsSingle(); 
            Container.Bind<IEffectFactory>().To<EffectFactory>().AsSingle();
            Container.Bind<IStatusFactory>().To<StatusFactory>().AsSingle();
            Container.Bind<IUltimateFactory>().To<UltimateFactory>().AsSingle();
            Container.Bind<IWindowFactory>().To<WindowFactory>().AsSingle();
        }

        private void BindGameStates()
        {
            Container.BindInterfacesAndSelfTo<BootstrapState>().AsSingle();
            Container.BindInterfacesAndSelfTo<StartConnectionState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingMenuState>().AsSingle();
            Container.BindInterfacesAndSelfTo<MenuState>().AsSingle();
            Container.BindInterfacesAndSelfTo<MatchmakingState>().AsSingle();
            Container.BindInterfacesAndSelfTo<LoadingBattleState>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleState>().AsSingle();
            Container.BindInterfacesAndSelfTo<BackGameplayConnectionState>().AsSingle();
        }

        private void BindInfrastructureServices()
        {
            Container.BindInterfacesTo<BootstrapInstaller>().FromInstance(this).AsSingle();
        }

        private void BindConnectionServices()
        {
            Container.Bind<PhotonConnector>().AsSingle();
            Container.BindInterfacesAndSelfTo<PhotonClientArgsProvider>()
                .FromComponentInNewPrefab(_photonClientArgsProvider)
                .AsSingle();
        }

        private void BindMatchmakingHandlers()
        {
            Container.BindInterfacesAndSelfTo<Matchmaker>().AsSingle();
            Container.BindInterfacesAndSelfTo<RoomWatcher>().AsSingle();
            Container.BindInterfacesAndSelfTo<BattleSimulationLoader>().AsSingle();
            Container.BindInterfacesAndSelfTo<PhotonRoomEventHandler>().AsSingle();
        }

        private void BindStateMachine()
        {
            Container.Bind<IGameStateMachine>().To<GameStateMachine>().AsSingle();
        }

        private void BindStateFactory()
        {
            Container.BindInterfacesAndSelfTo<StateFactory>().AsSingle();
        }

        private void BindCommonServices()
        {
            Container.Bind<ISceneLoader>().To<SceneLoader>().AsSingle();
            Container.Bind<ITimeService>().To<UnityTimeService>().AsSingle();
        }
    }
}