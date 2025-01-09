using Cysharp.Threading.Tasks;
using Zenject;
using UI.Views;
using Logic.Systems;

namespace UI.Models
{
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

        public override void OnEscape()
        {
            ReturnToPauseOrMenu();
        }

        public void SetModelToOpenAfterClose(IUIModel model)
        {
            _modelToOpenAfterClose = model;
        }

        private void ReturnToPauseOrMenu()
        {
            _uiSystem.Hide(this).Forget();

            if (_modelToOpenAfterClose != null)
            {
                _uiSystem.Show(_modelToOpenAfterClose).Forget();
            }
        }

        private void ToggleFullscreen(bool value)
        {
            _settings.SetFullscreenMode(value);
        }

        private void ChangeMusicVolume(float value)
        {
            _settings.SetMusicVolume(value);
        }

        private void ChangeSoundVolume(float value)
        {
            _settings.SetSoundVolume(value);
        }

        private void ChangeResolution(int index)
        {
            _settings.SetResolution(_settings.GetResolutionByIndex(index));
        }
    }
}
