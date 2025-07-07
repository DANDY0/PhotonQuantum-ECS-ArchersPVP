using Quantum;
using UnityEngine;

namespace Code.Gameplay.Arrow
{
    public class ArrowComponentsHolder: QuantumEntityViewComponent
    {
        [SerializeField] private GameObject _arrowObject;
        [SerializeField] private ArrowStatusEffectsVisual _arrowStatusEffectsVisual;

        public override void OnActivate(Frame frame)
        {
            _arrowObject.SetActive(true);
            _arrowStatusEffectsVisual.OnActivate(frame);
        }
    }
}