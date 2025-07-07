using Code.Connection;
using UnityEngine;

namespace Code.Menu.PlayerAppearance
{
    public class PlayerAppearanceModel
    {
        PhotonClientArgsProvider _photonClientArgsProvider;
        public const string NICKNAME_KEY = "PlayerNickname";
        public string Nickname { get; private set; }

        public void Init(PhotonClientArgsProvider photonClientArgsProvider)
        {
            _photonClientArgsProvider = photonClientArgsProvider;
            
            var nickName = PlayerPrefs.GetString(NICKNAME_KEY);
            photonClientArgsProvider.ConnectArgs.RuntimePlayers[0].PlayerNickname = nickName;
            
            SetNickname(nickName);
        }
        
        public void SetNickname(string newNickname)
        {
            Nickname = newNickname;
            
            _photonClientArgsProvider.ConnectArgs.RuntimePlayers[0].PlayerNickname = newNickname;
            PlayerPrefs.SetString(NICKNAME_KEY, newNickname);
            PlayerPrefs.Save();
        }

        public string LoadNickname()
        {
            return PlayerPrefs.GetString(NICKNAME_KEY, "Player");
        }
    }
}