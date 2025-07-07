using Code.Connection;

namespace Code.Menu.PlayerAppearance
{
    public class PlayerAppearanceController
    {
        private readonly PlayerAppearanceModel _model;
        private readonly PlayerAppearancePanel _view;

        public PlayerAppearanceController(PlayerAppearanceModel model,
            PlayerAppearancePanel view, PhotonClientArgsProvider photonClientArgsProvider)
        {
            _view = view;
            _model = model;
            _model.Init(photonClientArgsProvider);

            _view.InputField.onEndEdit.AddListener(OnEndEdit);
        }

        public void Open()
        {
            var currentNickname = _model.LoadNickname();
            _view.Show(currentNickname);
        }

        private void OnEndEdit(string newNickname)
        {
            _model.SetNickname(newNickname);
        }
    }
}