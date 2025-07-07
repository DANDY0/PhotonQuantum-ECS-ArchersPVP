using Zenject;

namespace Code.UI.Windows
{
    public class BaseController<TView>
        where TView : BaseWindow
    {
        [Inject] protected TView View;
    }
}