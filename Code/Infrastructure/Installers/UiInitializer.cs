using Code.UI.Windows;
using UnityEngine;
using Zenject;

namespace Code.Infrastructure.Installers
{
    public class UiInitializer : MonoBehaviour
    {
        public RectTransform UIRoot;
        [Inject] private IWindowFactory _windowFactory;

        [Inject]
        private void Construct(
            IWindowFactory windowFactory
        )
        {
            _windowFactory = windowFactory;
            _windowFactory.SetUIRoot(UIRoot);
        }
    }
}