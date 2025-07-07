using System;
using Quantum;
using UnityEngine;

namespace Code.Player
{
    public class PlayerAnimationsHandler : QuantumEntityViewComponent
    {
        [SerializeField] private Animator _animator;

        private static readonly int IsMoving = Animator.StringToHash("isMoving");
        private static readonly int Attack = Animator.StringToHash("attack");
        private static readonly int DamageTaken = Animator.StringToHash("getHit");
        private static readonly int Death = Animator.StringToHash("death");
        private static readonly int Respawned = Animator.StringToHash("respawned");

        private bool _isDummy;

        public override void OnActivate(Frame frame)
        {
            _isDummy = frame.Has<Dummy>(EntityRef);
        }
        
        public void OnPlayerIdle(EventPlayerIdle e)
        {
            if (e.Value == EntityRef) 
                _animator.SetBool(IsMoving, false);
        }

        public void OnPlayerRun(EventPlayerRun e)
        {
            if(_isDummy)
                return;
            
            if (e.Value == EntityRef) 
                _animator.SetBool(IsMoving, true);
        }

        public void OnPlayerAttack(EventPlayerAttack e)
        {
            if(_isDummy)
                return;
            
            if (e.Value == EntityRef) 
                _animator.SetTrigger(Attack);
        }

        public void OnDamageTaken(EventDamageTaken e)
        {
            if (e.Entity == EntityRef) 
                _animator.SetTrigger(DamageTaken);
        }

        public void OnPlayerDead(EventPlayerDead e)
        {
            if (e.Value == EntityRef) 
                _animator.SetTrigger(Death);
        }
        
        public void OnRespawned(EventPlayerRespawned e)
        {
            if (e.Value == EntityRef) 
                _animator.SetTrigger(Respawned);
        }
    }
}