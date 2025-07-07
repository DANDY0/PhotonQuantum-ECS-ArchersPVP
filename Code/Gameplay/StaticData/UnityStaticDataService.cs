using System;
using System.Collections.Generic;
using System.Linq;
using Code.Debuging;
using Code.UI.Windows;
using Code.UI.Windows.Configs;
using UnityEngine;

namespace Code.Gameplay.StaticData
{
    public class UnityStaticDataService : IUnityStaticDataService
    {
        private Dictionary<WindowId, GameObject> _windowPrefabsById;
        public DebugSettings DebugSettings { get; private set; }

        public void LoadAll()
        {
            LoadWindows();
            LoadDebugSettings();
        }

        public GameObject GetWindowPrefab(WindowId id) =>
            _windowPrefabsById.TryGetValue(id, out GameObject prefab)
                ? prefab
                : throw new Exception($"Prefab config for window {id} was not found");
        
        private void LoadWindows()
        {
            _windowPrefabsById = Resources
                .Load<WindowsConfig>("Configs/Windows/WindowsConfig")
                .WindowConfigs
                .ToDictionary(x => x.Id, x => x.Prefab);
        }
        
        private void LoadDebugSettings()
        {
            DebugSettings = Resources.Load<DebugSettings>("Configs/DebugSettings");
        }
    }
}