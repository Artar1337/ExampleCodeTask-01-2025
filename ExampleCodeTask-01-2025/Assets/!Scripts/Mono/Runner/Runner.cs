using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Runner : MonoBehaviour
{
    [Inject] private IUISystem _uiSystem;
    [Inject] private IResourcesSystem _resourcesSystem;
    [Inject] private IInstantiator _instantiator;
    [Inject] private IGameCycleSystem _gameCycleSystem;
    [Inject] private ILevelSystem _levelSystem;
    [Inject] private IAudioSystem _audioSystem;
    [Inject] private ILavaSystem _lavaSystem;
    [Inject] private ISettingsSystem _settingsSystem;
    [Inject] private IGameScoreSystem _scoreSystem;
    [Inject] private UIModelMenu _menuModel;
    [Inject] private UIModelLoading _loadingModel;

    private async void Start()
    {
        _uiSystem.Show(_loadingModel).Forget();
        await _resourcesSystem.Init();
        await _audioSystem.Init();
        _settingsSystem.Init();
        await _gameCycleSystem.Init();
        await _levelSystem.Init(_resourcesSystem, _instantiator);
        await _lavaSystem.Init();
        _scoreSystem.Init();

        _uiSystem.Hide(_loadingModel).Forget();
        _uiSystem.Show(_menuModel).Forget();
    }

    private void OnApplicationQuit()
    {
        _settingsSystem.SaveSettings();
    }
}
