using TMPro;
using UnityEngine;

namespace Code.UI
{
    public class NicknameBarView: MonoBehaviour
    {
        public TextMeshProUGUI _nicknameText;
        
        public void SetNickName(string nickname)
        {
            _nicknameText.text = nickname;
        }
    }
}