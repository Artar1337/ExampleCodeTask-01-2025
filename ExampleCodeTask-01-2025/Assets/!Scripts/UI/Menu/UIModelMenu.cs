using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class UIModelMenu : UIModel<UIViewMenu>
{
    public override string ViewName => "UI/Menu";

    [Inject] private UIModelAbout _aboutModel;
    [Inject] private UIModelSettings _settingsModel;
    [Inject] private UIModelPopup _popupModel;
    [Inject] private IGameCycleSystem _gameCycleSystem;
    [Inject] private ISettingsSystem _settingsSystem;
    [Inject] private IGameScoreSystem _gameScoreSystem;

    public override void OnInit()
    {
        _view.OnAuthorClick += OpenAuthorPage;
        _view.OnExitClick += OpenExitConfirmation;
        _view.OnSettingsClick += OpenSettings;
        _view.OnStartClick += Start;
        _view.OnRuToggleChanged += (x) => OnLanguageToggle(x, 1);
        _view.OnEnToggleChanged += (x) => OnLanguageToggle(x, 0);
        _view.InitToggles(_settingsSystem.LocaleIndex);
    }

    public override void OnShow()
    {
        _view.SetHiScoreText($"{"Record".Localize()}{_gameScoreSystem.HiScore}");
    }

    private void OnLanguageToggle(bool value, int languageIndex)
    {
        if (!value)
        {
            return;
        }

        _settingsSystem.SetLocaleByIndex(languageIndex);
        OnShow();
    }

    private void Start()
    {
        _gameCycleSystem.StartGame();
    }

    private void OpenSettings()
    {
        _uiSystem.Hide(this).Forget();
        _settingsModel.SetModelToOpenAfterClose(this);
        _uiSystem.Show(_settingsModel).Forget();
    }

    private void OpenAuthorPage()
    {
        _uiSystem.Hide(this).Forget();
        _uiSystem.Show(_aboutModel).Forget();
    }

    private void OpenExitConfirmation()
    {
        _uiSystem.Hide(this).Forget();

        _popupModel.Init("Exit".Localize(),
            "ExitConfirmText".Localize(),
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
