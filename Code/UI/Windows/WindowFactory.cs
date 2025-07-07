using System.Collections.Generic;
using Code.Gameplay.StaticData;
using Code.Menu;
using Code.UI.Gameplay;
using Code.UI.Gameplay.Intro;
using Code.UI.Gameplay.Windows.GameOver;
using Code.UI.Gameplay.Windows.GameplayDebug;
using Code.UI.Gameplay.Windows.MatchScore;
using Code.UI.Menu.Windows;
using Code.UI.Menu.Windows.Menu;
using Unity.VisualScripting;
using UnityEngine;
using Zenject;
using IInitializable = Zenject.IInitializable;

namespace Code.UI.Windows
{
    public class WindowFactory : IWindowFactory
    {
        private readonly IUnityStaticDataService _unityStaticData;
        private readonly IInstantiator _instantiator;
        private readonly DiContainer _container;
        private RectTransform _uiRoot;

        private readonly Dictionary<WindowId, BaseWindow> _windows = new();

        public WindowFactory(
            IUnityStaticDataService unityStaticData,
            IInstantiator instantiator,
            DiContainer container)
        {
            _unityStaticData = unityStaticData;
            _instantiator = instantiator;
            _container = container;
        }

        public void CreateStartConnection()
        {
            CreateWindow<LoadingConnectionToMasterWindow>(WindowId.LoadingConnectionToMaster);
        }
        
        public void CreateMenu()
        {
            CreateWindow<MenuWindowController, MenuWindow>(WindowId.Menu);
            CreateWindow<LoadingMatchmakingWindow>(WindowId.LoadingMatchmaking);
            CreateWindow<LoadingBackGameplayWindow>(WindowId.LoadingBackFromGameplay);
        }
        
        public void CreateBattle()
        {
            CreateWindow<MatchIntroWindowController, MatchIntroWindow>(WindowId.MatchIntro);
            CreateWindow<CountdownWindowController, CountdownWindow>(WindowId.CountDown);
            CreateWindow<MatchScoreWindowController, MatchScoreWindow>(WindowId.MatchScore);
            CreateWindow<GameOverWindowController, GameOverWindow>(WindowId.GameOver);
            CreateWindow<GameplayDebugWindowController, GameplayDebugWindow>(WindowId.GameplayDebug);
        }
        
        public void SetUIRoot(RectTransform uiRoot) =>
            _uiRoot = uiRoot;
        
        public BaseWindow GetWindow(WindowId windowId) => 
            _windows[windowId];

        private GameObject PrefabFor(WindowId id) =>
            _unityStaticData.GetWindowPrefab(id);

        private void CreateWindow<TController, TView>(WindowId windowId)
            where TController : BaseController<TView>
            where TView : BaseWindow
        {
            CreateWindow<TView>(windowId);
    
            if (!_container.HasBinding<TController>())
            {
                _container.BindInterfacesAndSelfTo<TController>().AsSingle();
                var controller = _container.Resolve<TController>();
                (controller as IInitializable)?.Initialize();
            }
            else
            {
                Debug.LogWarning($"[WindowFactory] Controller {typeof(TController).Name} is already bound. Skipping binding.");
            }
        }
        
        private void CreateWindow<TView>(WindowId windowId) where TView : BaseWindow
        {
            if (_windows.ContainsKey(windowId))
            {
                Debug.LogWarning($"[WindowFactory] Window {windowId} already exists, skipping creation.");
                return;
            }

            TView prefab = PrefabFor(windowId).GetComponent<TView>();
            TView window = _instantiator.InstantiatePrefabForComponent<TView>(prefab, _uiRoot);
            _container.Bind<TView>().FromInstance(window).AsSingle();
    
            window.Close();

            _windows.Add(windowId, window);
        }
    }
}