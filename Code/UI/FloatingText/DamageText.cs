using DG.Tweening;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;

namespace Code.UI.FloatingText
{
	public class DamageText : MonoBehaviour
	{
		[SerializeField] private TextMeshProUGUI _damageText;
		[SerializeField] private CanvasGroup _canvasGroup;

		public void Initialize(string text, Vector3 position, bool isCritical, DamageTextSettings settings)
		{
			transform.position = position;
			_damageText.text = text;
			_damageText.color = isCritical ? settings.criticalHitColor : settings.normalHitColor;

			float randomX = Random.Range(settings.randomOffsetRange.x, settings.randomOffsetRange.y);
			float randomY = Random.Range(settings.randomOffsetRange.x, settings.randomOffsetRange.y);
			Vector3 randomOffset = new Vector3(randomX, randomY, 0f);

			transform.DOMove(position + randomOffset + Vector3.up * settings.moveDistance, settings.duration).SetEase(Ease.OutQuad);

			_damageText.transform.DOScale(settings.scaleMultiplier, settings.duration * 0.3f).OnComplete(() =>
			{
				_damageText.transform.DOScale(1f, settings.duration * 0.7f);
			});

			_canvasGroup.DOFade(0, settings.duration).OnComplete(() => Destroy(gameObject));
		}
	}
}