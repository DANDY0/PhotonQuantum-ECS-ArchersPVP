using Code.Debuging;
using Code.UI.Windows;
using UnityEngine;

namespace Code.Gameplay.StaticData
{
    public interface IUnityStaticDataService
    {
        void LoadAll();
        
        GameObject GetWindowPrefab(WindowId id);
        DebugSettings DebugSettings { get; }
    }
}