using System.Collections.Generic;

namespace Code.UI.Windows
{
    public class WindowService : IWindowService
    {
        private readonly IWindowFactory _windowFactory;
        private readonly List<BaseWindow> _openedWindows = new();

        public WindowService(IWindowFactory windowFactory) =>
            _windowFactory = windowFactory;

        public BaseWindow Open(WindowId windowId)
        {
            BaseWindow window = _windowFactory.GetWindow(windowId);
            window.Open();
            _openedWindows.Add(window);
            return window;
        }

        public void Close(WindowId windowId)
        {
            BaseWindow window = _openedWindows.Find(x => x.Id == windowId);
            if (window is not null)
            {
                window.Close();
                _openedWindows.Remove(window);
            }
        }

        public bool IsWindowOpen(WindowId windowId)
        {
            return _openedWindows.Exists(x => x.Id == windowId);
        }
    }
}