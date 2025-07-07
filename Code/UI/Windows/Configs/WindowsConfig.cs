using System.Collections.Generic;
using UnityEngine;

namespace Code.UI.Windows.Configs
{
  [CreateAssetMenu(fileName = "WindowConfig", menuName = "Windows/Windows Config")]
  public class WindowsConfig : ScriptableObject
  {
    public List<WindowConfig> WindowConfigs;
  }
}