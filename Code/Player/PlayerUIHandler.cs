using Code.UI;
using Quantum;
using UnityEngine;

namespace Code.Player
{
    public class PlayerUIHandler: QuantumEntityViewComponent
    {
        [SerializeField] private Canvas _canvas;

        [SerializeField] private HealthBarView _healthBarView;
        [SerializeField] private NicknameBarView _nicknameBarView;

        public override void OnActivate(Frame frame)
        {
            PlayerRef player = VerifiedFrame.Get<PlayerLink>(EntityRef).Value;

            var nickname = VerifiedFrame.GetPlayerData(player).PlayerNickname;
            
            if (Game.PlayerIsLocal(player))
            {
                _nicknameBarView.SetNickName(VerifiedFrame.Has<Dummy>(EntityRef) 
                    ? "BOT" : nickname);
            }
            else
            {
                _nicknameBarView.SetNickName(nickname);
            }
            _healthBarView.SetMaxHp(VerifiedFrame.Get<MaxHp>(EntityRef).Value);
        }

        /// <summary>
        /// Updates the player UI elements based on the current game state.
        /// </summary>
        public override void OnUpdateView()
        {
            var currentHp = VerifiedFrame.Get<CurrentHp>(EntityRef);
            
            _healthBarView.SetHpValue(currentHp.Value);
        }

        public void SetVisibility(bool isVisible) => _canvas.enabled = isVisible;
    }
}