using System;
using System.Collections.Generic;
using Code.Connection;
using Code.Helpers;
using Quantum;
using TMPro;
using UnityEngine;
using Zenject;

namespace Code.UI.Menu.Windows
{
   	public class MenuAbilitiesSelection : MonoBehaviour
	{
		[SerializeField] private TMP_Dropdown[] _abilityDropdowns = Array.Empty<TMP_Dropdown>();
		[SerializeField] protected UnityEngine.UI.Button _backButton;

		private const string SaveKey = "SelectedAbilities";
		private readonly JsonSerializerService _serializer = new();
		
		[Inject] private PhotonClientArgsProvider _photonClientArgsProvider;

		public void Init()
		{
			SetupDropdowns();
			LoadSelectedAbilities();
			_backButton.onClick.AddListener(OnBackButtonPressed);
		}

		private void OnDestroy()
		{
			_backButton.onClick.RemoveListener(OnBackButtonPressed);
		}

		void SetupDropdowns()
		{
			foreach (var dropdown in _abilityDropdowns)
			{
				dropdown.ClearOptions();
				var options = new List<TMP_Dropdown.OptionData>();

				foreach (EAbilityCardId ability in Enum.GetValues(typeof(EAbilityCardId))) 
					options.Add(new TMP_Dropdown.OptionData(ability.ToString()));

				dropdown.AddOptions(options);
				dropdown.onValueChanged.AddListener(OnAbilitySelectionChanged);
			}
		}

		private void LoadSelectedAbilities()
		{
			if (PlayerPrefs.HasKey(SaveKey))
			{
				var json = PlayerPrefs.GetString(SaveKey);
				if (_serializer.TryDeserialize<AbilitySaveData>(json, out var saveData))
				{
					for (int i = 0; i < _abilityDropdowns.Length; i++)
					{
						var selected = saveData.SelectedAbilities[i];
						_abilityDropdowns[i].value = Array.IndexOf(Enum.GetValues(typeof(EAbilityCardId)), selected);
						_photonClientArgsProvider.RuntimeLocalPlayer.cardAbilityData[i].cardAbility = selected;
					}
				}
			}
		}

		private void OnAbilitySelectionChanged(int index)
		{
			for (int i = 0; i < _abilityDropdowns.Length; i++)
			{
				int selectedValue = _abilityDropdowns[i].value;
				EAbilityCardId selectedAbility = (EAbilityCardId)Enum.GetValues(typeof(EAbilityCardId)).GetValue(selectedValue);
				_photonClientArgsProvider.RuntimeLocalPlayer.cardAbilityData[i].cardAbility = selectedAbility;
			}
		}

		private void OnBackButtonPressed()
		{
			SaveSelectedAbilities();
			gameObject.SetActive(false);
		}

		private void SaveSelectedAbilities()
		{
			var selectedAbilities = new EAbilityCardId[_abilityDropdowns.Length];
			for (int i = 0; i < _abilityDropdowns.Length; i++)
			{
				int selectedValue = _abilityDropdowns[i].value;
				selectedAbilities[i] =
					(EAbilityCardId)Enum.GetValues(typeof(EAbilityCardId)).GetValue(selectedValue);
			}

			var saveData = new AbilitySaveData { SelectedAbilities = selectedAbilities };
			var json = _serializer.Serialize(saveData);
			PlayerPrefs.SetString(SaveKey, json);
			PlayerPrefs.Save();
		}
	}

}