using Quantum;
using Quantum.Collections;
using UnityEngine;

namespace Code.Gameplay.Arrow
{
    public class ArrowStatusEffectsVisual : MonoBehaviour
    {
        [SerializeField] private QuantumEntityView _entityView;
        [SerializeField] private ParticleSystem _arrow;

        [SerializeField] private GameObject _fireStatus;
        [SerializeField] private GameObject _freezeStatus;
        [SerializeField] private GameObject _poisonStatus;

        public void OnActivate(Frame frame)
        {
            SetStatusEffects(frame);
        }

        private void SetStatusEffects(Frame frame)
        {
            DisableAllStatusEffects();
            
            if (frame.Has<StatusSetups>(_entityView.EntityRef))
            {
                QListPtr<StatusSetup> statusSetups = frame.Get<StatusSetups>(_entityView.EntityRef).Value;
                if (frame.TryResolveList(statusSetups, out var statuses))
                {
                    if (statuses.Count > 0)
                    {
                        foreach (var status in statuses)
                            ActivateStatusEffect(status);
                    }
                    else
                        Debug.Log("NO statuses on arrow");
                }
                else
                    Debug.Log("Statuses not resolved (pointer likely invalid or empty)");
            }
        }

        private void DisableAllStatusEffects()
        {
            _fireStatus.SetActive(false);
            _freezeStatus.SetActive(false);
            _poisonStatus.SetActive(false);
        }

        private void ActivateStatusEffect(StatusSetup status)
        {
            switch (status.StatusTypeId)
            {
                case EStatusTypeId.Fire:
                    _fireStatus.SetActive(true);
                    break;
                case EStatusTypeId.Freeze:
                    _freezeStatus.SetActive(true);
                    break;
                case EStatusTypeId.Poison:
                    _poisonStatus.SetActive(true);
                    break;
                default:
                    Debug.LogWarning($"Unknown status: {status.StatusTypeId}");
                    break;
            }
        }
    }
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
    
}
