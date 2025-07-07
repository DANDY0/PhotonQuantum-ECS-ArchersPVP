using Code.Utils;
using TMPro;
using UnityEngine;

namespace Code.Menu
{
    public class MenuLoadingPanel: MonoSingleton<MenuLoadingPanel>
    {
        [SerializeField] private GameObject _content;
        [SerializeField] private TextMeshProUGUI _loadingDescription;
        
        public void Show(string text = "")
        {
            _content.SetActive(true);
            SetText(text);
        }
        
        public void Hide()
        {
            _content.SetActive(false);
        }

        //public, caz text can be changed while loading
        public void SetText(string text)
        {
            
        }
    }
}