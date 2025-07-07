using System;
using Photon.Deterministic;
using Quantum;

namespace Code.UI.FloatingText
{
	using UnityEngine;

	public class DamageTextSpawner : QuantumCallbacks
	{
		[SerializeField] private QuantumEntityView _entityView;

		[SerializeField] private DamageText _damageTextPrefab;
		[SerializeField] private DamageTextSettings _textSettings;
		[SerializeField] private RectTransform _spawnParent;

		private void Awake()
		{
			QuantumEvent.Subscribe<EventDamageTaken>(this, OnDamageTaken);
		}

		private void OnDamageTaken(EventDamageTaken e)
		{
			if (e.Entity == _entityView.EntityRef) 
				ShowDamageText(e.Value);
		}

		private void ShowDamageText(FP value, bool isCritical = false) {
			var damageText = Instantiate(_damageTextPrefab, _spawnParent);

			damageText.Initialize(value.ToString(), transform.position, isCritical, _textSettings);
		}
	}

}