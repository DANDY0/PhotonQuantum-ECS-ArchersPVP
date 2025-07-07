using Photon.Deterministic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Code.UI
{
    public class HealthBarView : MonoBehaviour
    {
        public Image _imageValue;
        public Image _backImage;
        public TextMeshProUGUI _textValues;
        
        private FP _maxHp;

        public void SetHpValue(FP currentHpValue)
        {
            _imageValue.fillAmount = currentHpValue.AsFloat / _maxHp.AsFloat;
            _textValues.text = $"{(int)currentHpValue}/{(int)_maxHp}" ;
        }

        public void SetMaxHp(FP value)
        {
            _maxHp = value;
        }
    }
}