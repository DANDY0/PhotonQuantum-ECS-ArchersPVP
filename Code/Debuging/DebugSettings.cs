using UnityEngine;
namespace Code.Debuging
{
    [CreateAssetMenu(fileName = "DebugSettings", menuName = "Config/Debug Settings")]
    public class DebugSettings : ScriptableObject
    {
        //Common
        
        //Menu
        
        //Gameplay
        public bool ShowDebugButton = true;
        public bool AllowOpenGameOverCode = true;
    }

}