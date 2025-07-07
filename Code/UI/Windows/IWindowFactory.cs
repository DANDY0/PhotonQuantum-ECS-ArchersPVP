using UnityEngine;

namespace Code.UI.Windows
{
  public interface IWindowFactory
  {
    public void SetUIRoot(RectTransform uiRoot);
    
    BaseWindow GetWindow(WindowId windowId);
    void CreateStartConnection();
    void CreateMenu();
    void CreateBattle();
  }
}