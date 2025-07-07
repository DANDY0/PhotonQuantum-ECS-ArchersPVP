using System;
using Quantum;
using UnityEngine;

namespace Code.Player
{
	//class holder for classes that need Events handling logic
	public class PlayerEventCallbacksHandler: QuantumCallbacks
	{
		[SerializeField] private PlayerAnimationsHandler _playerAnimationsHandler;
		[SerializeField] private PlayerAttackRangeView _playerAttackRangeView;
		[SerializeField] private PlayerEffectsVisual _playerEffectsVisual;

		private void Awake()
		{
			//states changes
			QuantumEvent.Subscribe<EventPlayerIdle>(this, OnPlayerIdle);
			QuantumEvent.Subscribe<EventPlayerRun>(this, OnPlayerRun);
			QuantumEvent.Subscribe<EventPlayerAttack>(this, OnPlayerAttack);
			
			//status effects
			QuantumEvent.Subscribe<EventFireApplied>(this, OnFireApplied);
			QuantumEvent.Subscribe<EventFireUnApplied>(this, OnFireUnApplied);
			QuantumEvent.Subscribe<EventFreezeApplied>(this, OnFreezeApplied);
			QuantumEvent.Subscribe<EventFreezeUnApplied>(this, OnFreezeUnApplied);
			QuantumEvent.Subscribe<EventPoisonApplied>(this, OnPoisonApplied);
			QuantumEvent.Subscribe<EventPoisonUnApplied>(this, OnPoisonUnApplied);
			
			//damage,death
			QuantumEvent.Subscribe<EventDamageTaken>(this, OnDamageTaken);
			QuantumEvent.Subscribe<EventPlayerDead>(this, OnPlayerDead);
			QuantumEvent.Subscribe<EventPlayerRespawned>(this, OnPlayerRespawned);
			
		}

		private void OnPlayerAttack(EventPlayerAttack callback)
		{
			_playerAnimationsHandler.OnPlayerAttack(callback);
		}

		private void OnPlayerRun(EventPlayerRun callback)
		{
			_playerAnimationsHandler.OnPlayerRun(callback);
		}

		private void OnPlayerIdle(EventPlayerIdle callback)
		{
			_playerAnimationsHandler.OnPlayerIdle(callback);
		}

		private void OnFreezeApplied(EventFreezeApplied callback)
		{
			_playerEffectsVisual.OnFreezeApplied(callback);
		}

		private void OnFreezeUnApplied(EventFreezeUnApplied callback)
		{
			_playerEffectsVisual.OnFreezeUnApplied(callback);
		}

		private void OnFireApplied(EventFireApplied callback)
		{
			_playerEffectsVisual.OnFireApplied(callback);
		}

		private void OnFireUnApplied(EventFireUnApplied callback)
		{
			_playerEffectsVisual.OnFireUnApplied(callback);
		}

		private void OnPoisonApplied(EventPoisonApplied callback)
		{
			_playerEffectsVisual.OnPoisonApplied(callback);
		}

		private void OnPoisonUnApplied(EventPoisonUnApplied callback)
		{
			_playerEffectsVisual.OnPoisonUnApplied(callback);
		}

		private void OnDamageTaken(EventDamageTaken callback)
		{
			_playerAnimationsHandler.OnDamageTaken(callback);
			_playerEffectsVisual.OnDamageTaken(callback);
		}

		private void OnPlayerDead(EventPlayerDead callback)
		{
			_playerAnimationsHandler.OnPlayerDead(callback);
			_playerAttackRangeView.Deactivate();
			_playerEffectsVisual.OnDead(callback);
		}

		private void OnPlayerRespawned(EventPlayerRespawned callback)
		{
			_playerAnimationsHandler.OnRespawned(callback);
			_playerAttackRangeView.Activate();
		}
	}
}