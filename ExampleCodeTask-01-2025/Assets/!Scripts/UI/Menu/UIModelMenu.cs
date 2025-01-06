using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class UIModelMenu : UIModel<UIViewMenu>
{
    public override string ViewName => "UI/Menu";

    [Inject] private UIModelAbout _aboutModel;
    [Inject] private UIModelSettings _settingsModel;
    [Inject] private UIModelPopup _popupModel;
    [Inject] private UIModelHud _hudModel;

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
            "Действительно хотите выйти из игры?",
            () =>
            {
#if UNITY_EDITOR
                UnityEditor.EditorApplication.isPlaying = false;
#endif
                Application.Quit();
            },
            () =>
            {
                _uiSystem.Show(this).Forget();
            });

        _uiSystem.Show(_popupModel).Forget();
    }
}
