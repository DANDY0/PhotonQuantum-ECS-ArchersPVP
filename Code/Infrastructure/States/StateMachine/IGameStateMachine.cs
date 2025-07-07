using System;
using Code.Infrastructure.States.StateInfrastructure;

namespace Code.Infrastructure.States.StateMachine
{
  public interface IGameStateMachine 
  {
    void Enter<TState>() where TState : class, IState;
    void Enter<TState, TPayload>(TPayload payload) where TState : class, IPayloadState<TPayload>;
    event Action<IExitableState> StateChangedEvent;
    IExitableState ActiveState { get; }
  }
}