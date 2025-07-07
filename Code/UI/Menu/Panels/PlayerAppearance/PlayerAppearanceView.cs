using TMPro;
using UnityEngine;

namespace Code.Menu.PlayerAppearance
{
    public class PlayerAppearancePanel: MonoBehaviour
    {
        public TMP_InputField InputField;
        public GameObject Root;
        
        public void Show(string currentNickname)
        {
            InputField.text = currentNickname;
            Show();
        }

        private void Show()
        {
            Root.SetActive(true);
        }

        public void Hide()
        {
            Root.SetActive(false);
        }
    }
}