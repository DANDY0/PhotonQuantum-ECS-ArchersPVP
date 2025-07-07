using RSG;

namespace Code.Infrastructure.States.StateInfrastructure
{
    public class SimpleBattleState: IBattleState
    {
        public virtual void Enter()
        {
      
        }

        protected virtual void Exit()
        {
      
        }
    
    
        IPromise IExitableState.BeginExit()
        {
            Exit();
            return Promise.Resolved();
        }

        void IExitableState.EndExit() { }
    }
}