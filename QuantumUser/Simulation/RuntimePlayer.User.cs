using System;
using System.Collections.Generic;

namespace Quantum
{
	public partial class RuntimePlayer
	{
		public CharacterData characterData;
		public CardAbilityData[] cardAbilityData = Array.Empty<CardAbilityData>();
		public bool createDummy;
	}

	[Serializable]
	public class CharacterData
	{
		public CharacterName characterName;
		public int level;
	}

	[Serializable]
	public class CardAbilityData
	{
		public EAbilityCardId cardAbility;
		public int level;
	}

	public enum CharacterName
	{
		Archer, // default one
		Leon, // no character
	}
}

