namespace Code.UI.Windows
{
    public interface IWindowService
    {
        public BaseWindow Open(WindowId windowId);
        void Close(WindowId windowId);
        bool IsWindowOpen(WindowId windowId);
    }
}