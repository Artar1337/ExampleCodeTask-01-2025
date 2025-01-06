using Cysharp.Threading.Tasks;
using UnityEngine;

public class UIModelSettings : UIModel<UIViewSettings>
{
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
        // заполнить настройками
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

    }

    public void ChangeMusicVolume(float value)
    {

    }

    public void ChangeSoundVolume(float value)
    {

    }

    public void ChangeResolution(int index)
    {

    }
}
