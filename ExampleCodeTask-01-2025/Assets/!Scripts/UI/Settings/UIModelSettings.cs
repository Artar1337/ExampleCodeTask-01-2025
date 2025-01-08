using Cysharp.Threading.Tasks;
using Zenject;

public class UIModelSettings : UIModel<UIViewSettings>
{
    [Inject] private ISettingsSystem _settings;

    public override string ViewName => "UI/Settings";

    private IUIModel _modelToOpenAfterClose = null;

    public override void OnInit()
    {
        _view.OnExitClick += ReturnToPauseOrMenu;
        _view.OnFullscreenToggle += ToggleFullscreen;
        _view.OnMusicSliderChanged += ChangeMusicVolume;
        _view.OnSoundSliderChanged += ChangeSoundVolume;
        _view.OnResolutionIndexChanged += ChangeResolution;
    }

    public override void OnShow()
    {
        _view.InitializeView(_settings.Fullscreen,
            _settings.MusicVolume, _settings.SoundVolume,
            _settings.Resolutions,
            _settings.GetIndexByResolution(_settings.Resolution));
    }

    public override void OnHide()
    {
        _settings.SaveSettings();
    }

    public void SetModelToOpenAfterClose(IUIModel model)
    {
        _modelToOpenAfterClose = model;
    }

    public void ReturnToPauseOrMenu()
    {
        _uiSystem.Hide(this).Forget();

        if (_modelToOpenAfterClose != null)
        {
            _uiSystem.Show(_modelToOpenAfterClose).Forget();
        }
    }

    public void ToggleFullscreen(bool value)
    {
        _settings.SetFullscreenMode(value);
    }

    public void ChangeMusicVolume(float value)
    {
        _settings.SetMusicVolume(value);
    }

    public void ChangeSoundVolume(float value)
    {
        _settings.SetSoundVolume(value);
    }

    public void ChangeResolution(int index)
    {
        _settings.SetResolution(_settings.GetResolutionByIndex(index));
    }
}
