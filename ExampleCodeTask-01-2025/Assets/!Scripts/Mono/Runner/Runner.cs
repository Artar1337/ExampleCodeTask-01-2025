using Cysharp.Threading.Tasks;
using Logic.Systems;
using UI.Models;
using UnityEngine;
using Zenject;

namespace Logic.Mono
{
    public class Runner : MonoBehaviour
    {
        [Inject] private IUISystem _uiSystem;
        [Inject] private IPlayerMovementSystem _movementSystem;
        [Inject] private IResourcesSystem _resourcesSystem;
        [Inject] private IGameCycleSystem _gameCycleSystem;
        [Inject] private ILevelSystem _levelSystem;
        [Inject] private IAudioSystem _audioSystem;
        [Inject] private ILavaSystem _lavaSystem;
        [Inject] private ISettingsSystem _settingsSystem;
        [Inject] private IEscapePressHandler _escapePressHandler;
        [Inject] private IGameScoreSystem _scoreSystem;
        [Inject] private UIModelMenu _menuModel;
        [Inject] private UIModelLoading _loadingModel;

        private async void Start()
        {
            _uiSystem.Show(_loadingModel).Forget();

            await _resourcesSystem.Init();
            await _audioSystem.Init();
            _settingsSystem.Init();
            _gameCycleSystem.Init();
            await _movementSystem.Init();
            await _levelSystem.Init();
            await _lavaSystem.Init();
            _scoreSystem.Init();
            _escapePressHandler.Start();

            _uiSystem.Hide(_loadingModel).Forget();
            _uiSystem.Show(_menuModel).Forget();
        }

        private void OnApplicationQuit()
        {
            _settingsSystem.SaveSettings();
        }
    }
}
