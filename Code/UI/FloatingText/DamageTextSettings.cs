using UnityEngine;

namespace Code.UI.FloatingText
{
	[System.Serializable]
	public class DamageTextSettings
	{
		public float moveDistance = 2f;
		public float duration = 1f;
		public float scaleMultiplier = 1.5f;
		public Vector2 randomOffsetRange = new Vector2(-1f, 1f);
		public Color criticalHitColor = Color.red;
		public Color normalHitColor = Color.white;
	}
}