using Cysharp.Threading.Tasks;
using Zenject;

public class UIModelPause : UIModel<UIViewPause>
{
    public override string ViewName => "UI/Pause";

    [Inject] private UIModelAbout _aboutModel;
    [Inject] private UIModelSettings _settingsModel;
    [Inject] private UIModelPopup _popupModel;
    [Inject] private UIModelHud _hudModel;
    [Inject] private UIModelGameOver _gameOverModel;

    public override void OnInit()
    {
        _view.OnAuthorClick += OpenAuthorPage;
        _view.OnExitClick += OpenExitConfirmation;
        _view.OnSettingsClick += OpenSettings;
        _view.OnStartClick += Start;
    }

    public void Start()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_hudModel).Forget();
    }

    public void OpenSettings()
    {
        _uiSystem.Hide(this).Forget();
        _settingsModel.SetModelToOpenAfterClose(this);
        _uiSystem.Show(_settingsModel).Forget();
    }

    public void OpenAuthorPage()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_aboutModel).Forget();
    }

    public void OpenExitConfirmation()
    {
        _uiSystem.Hide(this).Forget();

        _popupModel.Init("Выход",
            "Действительно хотите выйти в меню?",
            () =>
            {
                // TODO: автосмерть
                _uiSystem.Show(_gameOverModel).Forget();
            },
            () =>
            {
                _uiSystem.Show(this).Forget();
            });

        _uiSystem.Show(_popupModel).Forget();
    }
}
