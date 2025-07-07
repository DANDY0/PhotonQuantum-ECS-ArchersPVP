using Code.Infrastructure.Loading;
using Code.Infrastructure.States.StateInfrastructure;
using Code.Menu;
using Code.UI.Windows;
using UnityEngine;

namespace Code.Infrastructure.States.GameStates
{
    public class MenuState : SimpleState
    {
        private readonly IWindowService _windowService;
        private readonly IWindowFactory _windowFactory;
        
        private bool _isCreated;

        public MenuState(IWindowService windowService, IWindowFactory windowFactory)
        {
            _windowService = windowService;
            _windowFactory = windowFactory;
        }

        public override void Enter()
        {
            if (!_isCreated)
            {
                _windowFactory.CreateMenu();         
                _isCreated = true;
            }
            
            // Debug.Log("Entered MenuState state");
            _windowService.Open(WindowId.Menu);
        }
    }
}