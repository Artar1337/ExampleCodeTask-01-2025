using Cysharp.Threading.Tasks;
using Zenject;

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

    public void Start()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_hudModel).Forget();
        _gameCycleSystem.SetPause(false);
    }

    public void OpenSettings()
    {
        _uiSystem.Hide(this).Forget();
        _settingsModel.SetModelToOpenAfterClose(this);
        _uiSystem.Show(_settingsModel).Forget();
    }

    public void OpenExitConfirmation()
    {
        _uiSystem.Hide(this).Forget();

        _popupModel.Init("Выход",
            "Действительно хотите выйти в меню?",
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
