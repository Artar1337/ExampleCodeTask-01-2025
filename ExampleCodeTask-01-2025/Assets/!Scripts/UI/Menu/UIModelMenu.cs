using Cysharp.Threading.Tasks;
using Zenject;

public class UIModelMenu : UIModel<UIViewMenu>
{
    public override string ViewName => "UI/Menu";

    [Inject] private UIModelAbout _aboutModel;

    public override void OnInit()
    {
        _view.OnAuthorClick += OpenAuthorPage;
        _view.OnExitClick += OpenExitConfirmation;
        _view.OnSettingsClick += OpenSettings;
        _view.OnStartClick += Start;
    }

    public override void OnShow()
    {
        _view.SetHiScoreText("Рекорд: 1222");
    }

    public void Start()
    {

    }

    public void OpenSettings()
    {
        _uiSystem.Hide(this).Forget();
        //_uiSystem.Show(_aboutModel).Forget();
    }

    public void OpenAuthorPage()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_aboutModel).Forget();
    }

    public void OpenExitConfirmation()
    {
        _uiSystem.Hide(this).Forget();
        //_uiSystem.Show(_aboutModel).Forget();
    }
}
