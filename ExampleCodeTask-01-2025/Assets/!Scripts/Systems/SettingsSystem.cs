using System.Linq;
using UnityEngine;
using UnityEngine.Localization.Settings;
using Zenject;

public interface ISettingsSystem
{
    float MusicVolume { get; }
    float SoundVolume { get; }
    Resolution Resolution { get; }
    Resolution[] Resolutions { get; }
    bool Fullscreen { get; }
    int LocaleIndex { get; }

    void SetFullscreenMode(bool value);
    void SetResolution(Resolution resolution);
    void SetSoundVolume(float value);
    void SetMusicVolume(float value);
    void Init();
    void SaveSettings();
    void SetLocaleByIndex(int index);
    Resolution GetResolutionByIndex(int index);
    int GetIndexByResolution(Resolution resolution);

}

public class SettingsSystem : ISettingsSystem
{
    private const string ResolutionWKey = "ResolutionW";
    private const string ResolutionHKey = "ResolutionH";
    private const string ResolutionRateKey = "ResolutionRate";
    private const string FullscreenKey = "Fullscreen";
    private const string MusicVolKey = "MusicVolume";
    private const string SoundVolKey = "SoundVolume";
    private const string LocaleKey = "Locale";

    [Inject] private IAudioSystem _audioSystem;

    public Resolution[] Resolutions { get; private set; } = Screen.resolutions;
    public float MusicVolume { get; private set; }
    public float SoundVolume { get; private set; }
    public Resolution Resolution { get; private set; }
    public bool Fullscreen { get; private set; }

    public int LocaleIndex { get; private set; }

    public void Init()
    {
        MusicVolume = PlayerPrefs.GetFloat(MusicVolKey, 1f);
        SoundVolume = PlayerPrefs.GetFloat(SoundVolKey, 1f);
        Fullscreen = PlayerPrefs.GetInt(FullscreenKey, 1) > 0;

        var defaultResolution = GetDefaultResolution();
        Resolution = new()
        {
            width = PlayerPrefs.GetInt(ResolutionWKey, defaultResolution.width),
            height = PlayerPrefs.GetInt(ResolutionHKey, defaultResolution.height),
            refreshRateRatio = new RefreshRate()
            {
                numerator = (uint)PlayerPrefs.GetFloat(ResolutionRateKey,
                (float)defaultResolution.refreshRateRatio.value),
                denominator = 1
            },
        };

        LocaleIndex = PlayerPrefs.GetInt(LocaleKey, 0);

        SetMusicVolume(MusicVolume);
        SetSoundVolume(SoundVolume);
        SetResolution(Resolution);
        SetFullscreenMode(Fullscreen);
        SetLocaleByIndex(LocaleIndex);
    }

    public void SetFullscreenMode(bool value)
    {
        Fullscreen = value;
        Screen.fullScreen = value;
        SetResolution(Resolution);
    }

    public void SetMusicVolume(float value)
    {
        MusicVolume = value;
        _audioSystem.SetMusicMixerVolume(value);
    }

    public void SetSoundVolume(float value)
    {
        SoundVolume = value;
        _audioSystem.SetSoundMixerVolume(value);
    }

    public void SetResolution(Resolution resolution)
    {
        Resolution = resolution;
        Screen.SetResolution(resolution.width, resolution.height,
            Fullscreen ? FullScreenMode.FullScreenWindow : FullScreenMode.Windowed,
            resolution.refreshRateRatio);
    }

    public void SaveSettings()
    {
        PlayerPrefs.SetFloat(MusicVolKey, MusicVolume);
        PlayerPrefs.SetFloat(SoundVolKey, SoundVolume);
        PlayerPrefs.SetInt(FullscreenKey, Fullscreen ? 1 : 0);
        PlayerPrefs.SetInt(ResolutionWKey, Resolution.width);
        PlayerPrefs.SetInt(ResolutionHKey, Resolution.height);
        PlayerPrefs.SetFloat(ResolutionRateKey, (float)Resolution.refreshRateRatio.value);
    }

    public Resolution GetResolutionByIndex(int index)
    {
        if (index < 0 || index > Resolutions.Length - 1)
        {
            Debug.LogError($"Can't get resolution index = {index}. Settings default one instead");
            return GetDefaultResolution();
        }

        return Resolutions[index];
    }

    private Resolution GetDefaultResolution()
    {
        return new()
        {
            width = Screen.width,
            height = Screen.height,
            refreshRateRatio = new RefreshRate()
            {
                numerator = (uint)Resolutions.Select(x => x.refreshRateRatio).Max().value,
                denominator = 1
            },
        };
    }

    public int GetIndexByResolution(Resolution resolution)
    {
        for (int i = 0; i < Resolutions.Length; i++)
        {
            if (Resolutions[i].width == Resolution.width &&
                Resolutions[i].height == Resolution.height &&
                Mathf.Approximately((float)Resolutions[i].refreshRateRatio.value,
                (float)Resolution.refreshRateRatio.value))
            {
                return i;
            }
        }

        return Resolutions.Length - 1;
    }

    public void SetLocaleByIndex(int index)
    {
        LocaleIndex = index;

        LocalizationSettings.SelectedLocale =
            LocalizationSettings.AvailableLocales.Locales[index];
    }
}
