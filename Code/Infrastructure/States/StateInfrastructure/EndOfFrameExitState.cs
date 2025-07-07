using RSG;

namespace Code.Infrastructure.States.StateInfrastructure
{
    public class EndOfFrameExitState : IState, IUpdateable
    {
        private Promise _exitPromise;
        private bool ExitWasRequested => _exitPromise != null;


        public virtual void Enter()
        {
        }

        protected virtual void ExitOnEndOfFrame()
        {
            
        }

        protected virtual void OnUpdate() { }

        IPromise IExitableState.BeginExit()
        {
            _exitPromise = new Promise();
            return _exitPromise;
        }

        void IExitableState.EndExit()
        {
            ExitOnEndOfFrame();
            ClearExitPromise();
        }

        private void ClearExitPromise()
        {
            _exitPromise = null;
        }

        void IUpdateable.Update()
        {
            if (!ExitWasRequested) 
                OnUpdate();
            
            if (ExitWasRequested) 
                ResolveExitPromise();
        }

        private void ResolveExitPromise()
        {
            _exitPromise?.Resolve();
        }
    }
}