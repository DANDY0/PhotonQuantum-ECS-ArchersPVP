using DG.Tweening;
using Quantum;
using UnityEngine;

namespace Code.Player
{
    public class PlayerEffectsVisual : MonoBehaviour
    {
        [SerializeField] private QuantumEntityView _entityView;
        
        [SerializeField] private SkinnedMeshRenderer _head;
        [SerializeField] private SkinnedMeshRenderer _body;
        [SerializeField] private Color _damageTakeColor = Color.red;
        [SerializeField] private float _damageEffectDuration = 0.5f;

        [SerializeField] private GameObject _fireEffect;
        [SerializeField] private GameObject _frostEffect;
        [SerializeField] private GameObject _poisonEffect;

        [SerializeField] private ParticleSystem _deadParticleSystem;

        private Sequence _headSequence;
        private Sequence _bodySequence;

        public void OnDamageTaken(EventDamageTaken e)
        {
            if (e.Entity != _entityView.EntityRef)
                return;

            _headSequence = ApplyColorChange(_headSequence, _head, Color.white, _damageTakeColor, _damageEffectDuration);
            _bodySequence = ApplyColorChange(_bodySequence, _body, Color.white, _damageTakeColor, _damageEffectDuration);
        }

        public void OnFireApplied(EventFireApplied e)
        {
            if (e.Value != _entityView.EntityRef)
                return;

            _fireEffect.SetActive(true);
        }

        public void OnFireUnApplied(EventFireUnApplied e)
        {
            if (e.Value != _entityView.EntityRef)
                return;

            _fireEffect.SetActive(false);
        }

        public void OnFreezeApplied(EventFreezeApplied e)
        {
            if (e.Value != _entityView.EntityRef)
                return;

            _frostEffect.SetActive(true);
        }

        public void OnFreezeUnApplied(EventFreezeUnApplied e)
        {
            if (e.Value != _entityView.EntityRef)
                return;

            _frostEffect.SetActive(false);
        }
        
        public void OnPoisonApplied(EventPoisonApplied e)
        {
            if (e.Value != _entityView.EntityRef)
                return;

            _poisonEffect.SetActive(true);
        }
        
        public void OnPoisonUnApplied(EventPoisonUnApplied e)
        {
            if (e.Value != _entityView.EntityRef)
                return;

            _poisonEffect.SetActive(false);
        }
        
        public void OnDead(EventPlayerDead e)
        {
            if (e.Value != _entityView.EntityRef)
                return;
            
            _deadParticleSystem.Play();
        }

        private Sequence ApplyColorChange(Sequence sequence, SkinnedMeshRenderer skinRenderer,
            Color originalColor, Color targetColor, float duration)
        {
            sequence?.Kill();

            var material = skinRenderer.materials[0];

            return DOTween.Sequence()
                .Append(material.DOColor(targetColor, duration / 2))
                .AppendInterval(duration)
                .Append(material.DOColor(originalColor, duration / 2))
                .OnKill(() => sequence = null);
        }
    }
}
