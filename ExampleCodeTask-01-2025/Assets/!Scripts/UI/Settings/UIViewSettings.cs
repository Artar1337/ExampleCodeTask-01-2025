using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIViewSettings : UIViewFadable
{
    [SerializeField] private Slider _soundSlider;
    [SerializeField] private Slider _musicSlider;
    [SerializeField] private TMP_Dropdown _resolutionDropdown;
    [SerializeField] private Toggle _fullScreenToggle;
    [SerializeField] private Button _exitButton;

    public event Action<float> OnSoundSliderChanged;
    public event Action<float> OnMusicSliderChanged;
    public event Action<int> OnResolutionIndexChanged;
    public event Action<bool> OnFullscreenToggle;
    public event Action OnExitClick;

    private void Awake()
    {
        _soundSlider.onValueChanged.AddListener((x) => OnSoundSliderChanged?.Invoke(x));
        _musicSlider.onValueChanged.AddListener((x) => OnMusicSliderChanged?.Invoke(x));
        _resolutionDropdown.onValueChanged.AddListener((x) => OnResolutionIndexChanged?.Invoke(x));
        _fullScreenToggle.onValueChanged.AddListener((x) => OnFullscreenToggle?.Invoke(x));
        _exitButton.onClick.AddListener(() => OnExitClick?.Invoke());
    }

    public void InitializeView(bool fullscreen, float musicVol, float soundVol,
        Resolution[] resolutions, int currentResolutionIndex)
    {
        _soundSlider.SetValueWithoutNotify(soundVol);
        _musicSlider.SetValueWithoutNotify(musicVol);
        _fullScreenToggle.SetIsOnWithoutNotify(fullscreen);

        _resolutionDropdown.SetValueWithoutNotify(0);
        _resolutionDropdown.ClearOptions();

        foreach (var res in resolutions)
        {
            string option = $"{res.width}x{res.height}";

            _resolutionDropdown.options.Add(new TMP_Dropdown.OptionData() { text = option });
        }

        _resolutionDropdown.SetValueWithoutNotify(currentResolutionIndex);
    }
}
