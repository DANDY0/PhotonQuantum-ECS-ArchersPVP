using UnityEngine.Rendering.PostProcessing;

namespace Code.Player
{
    using UnityEngine;
    using Quantum;

    public class PlayerAttackRangeView : QuantumEntityViewComponent
    {
        [SerializeField] private Gradient _myColor;
        [SerializeField] private Gradient _enemyColor;
        
        private LineRenderer _lineRenderer;

        private float _currentRadius;
        private float _targetRadius; 
        private float _lerpSpeed = 20f;
        
        private bool _isLocal;
        
        public override void OnActivate(Frame frame)
        {
            _lineRenderer = GetComponent<LineRenderer>();
            _currentRadius = 0f;
            
            _isLocal = Game.PlayerIsLocal(VerifiedFrame.Get<PlayerLink>(_entityView.EntityRef).Value);
            
            Activate();
            SetView(_isLocal);
        }

        public override void OnUpdateView()
        {
            if (VerifiedFrame.Has<AttackRange>(EntityRef))
            {
                var attackRange = VerifiedFrame.Get<AttackRange>(EntityRef);
                _targetRadius = attackRange.CurrentRange.AsFloat;
                _currentRadius = Mathf.Lerp(_currentRadius, _targetRadius, Time.deltaTime * _lerpSpeed);
                UpdateRadiusVisual(_currentRadius);
            }
            else
                _lineRenderer.enabled = false;
        }

        public void SetVisibility(bool isVisible) => _lineRenderer.enabled = isVisible;

        public void Activate()
        {
            if (_lineRenderer != null) 
                _lineRenderer.enabled = true;
        }

        public void Deactivate()
        {
            if (_lineRenderer != null) 
                _lineRenderer.enabled = false;
        }

        private void SetView(bool isLocal)
        {
            _lineRenderer.colorGradient = isLocal ? _myColor : _enemyColor;
        }

        private void UpdateRadiusVisual(float radius)
        {
            int segments = 64;
            _lineRenderer.positionCount = segments + 1;
            for (int i = 0; i <= segments; i++)
            {
                float angle = i * Mathf.PI * 2 / segments;
                float x = Mathf.Cos(angle) * radius;
                float z = Mathf.Sin(angle) * radius;
                _lineRenderer.SetPosition(i, new Vector3(x, 0, z));
            }
        }
    }
}
