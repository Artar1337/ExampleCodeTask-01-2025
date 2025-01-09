using Cysharp.Threading.Tasks;
using Zenject;
using UI.Views;
using Logic.Systems;

namespace UI.Models
{
    public class UIModelPause : UIModel<UIViewPause>
    {
        public override string ViewName => "UI/Pause";

        [Inject] private UIModelSettings _settingsModel;
        [Inject] private UIModelPopup _popupModel;
        [Inject] private UIModelHud _hudModel;
        [Inject] private IGameCycleSystem _gameCycleSystem;

        public override void OnInit()
        {
            _view.OnExitClick += OpenExitConfirmation;
            _view.OnSettingsClick += OpenSettings;
            _view.OnStartClick += Start;
        }

        public override void OnShow()
        {
            _gameCycleSystem.SetPause(true);
        }

        public override void OnEscape()
        {
            Start();
        }

        private void Start()
        {
            _uiSystem.Hide(this).Forget();
            _uiSystem.Show(_hudModel).Forget();
            _gameCycleSystem.SetPause(false);
        }

        private void OpenSettings()
        {
            _uiSystem.Hide(this).Forget();
            _settingsModel.SetModelToOpenAfterClose(this);
            _uiSystem.Show(_settingsModel).Forget();
        }

        private void OpenExitConfirmation()
        {
            _uiSystem.Hide(this).Forget();

            _popupModel.Init("Exit".Localize(),
                "ExitMenuConfirmText".Localize(),
                () =>
                {
                    _gameCycleSystem.EndGame();
                },
                () =>
                {
                    _uiSystem.Show(this).Forget();
                });

            _uiSystem.Show(_popupModel).Forget();
        }
    }
}
