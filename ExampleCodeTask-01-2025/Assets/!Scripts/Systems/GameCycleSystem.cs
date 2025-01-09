using Cysharp.Threading.Tasks;
using System;
using System.Collections.Generic;
using UI.Models;
using UnityEngine;
using Zenject;

namespace Logic.Systems
{
    public interface IGameCycleSystem
    {
        event Action OnGameOver;
        event Action OnGameStart;
        event Action OnGameInit;
        event Action<bool> OnPauseChange;
        bool Paused { get; }

        void SetPause(bool paused);
        UniTask StartGame();
        void EndGame();
        void CreateLevel();
        void Init();
    }

    public class GameCycleSystem : IGameCycleSystem
    {
        [Inject] private IPlayerMovementSystem _playerMovementSystem;
        [Inject] private IUISystem _uiSystem;
        [Inject] private IGameScoreSystem _scoreSystem;
        [Inject] private ILevelSystem _levelSystem;
        [Inject] private ILavaSystem _lavaSystem;
        [Inject] private UIModelGameOver _gameOverModel;
        [Inject] private UIModelHud _hudModel;

        public bool Paused { get; private set; }

        public event Action OnGameOver;
        public event Action OnGameStart;
        public event Action OnGameInit;
        public event Action<bool> OnPauseChange;

        public void EndGame()
        {
            SetPause(true);
            var setCopy = new HashSet<IUIModel>(_uiSystem.ShownModels);

            foreach (var model in setCopy)
            {
                _uiSystem.Hide(model).Forget();
            }

            _uiSystem.Show(_gameOverModel).Forget();

            OnGameOver?.Invoke();
        }

        public void Init()
        {
            SetPause(true);
            OnGameInit?.Invoke();
        }

        public void SetPause(bool paused)
        {
            Paused = paused;
            Physics.simulationMode = paused ?
                SimulationMode.Script : SimulationMode.FixedUpdate;

            OnPauseChange?.Invoke(paused);
        }

        public void CreateLevel()
        {
            // Reset player and lava
            _playerMovementSystem.ResetBall();
            _lavaSystem.Reset();
            // Load level
            _levelSystem.Generate();
        }

        public async UniTask StartGame()
        {
            _scoreSystem.ResetScore();
            // Hide all ui
            var setCopy = new HashSet<IUIModel>(_uiSystem.ShownModels);
        
            foreach (var model in setCopy)
            {
                _uiSystem.Hide(model).Forget();
            }

            CreateLevel();

            await _uiSystem.Show(_hudModel);
            await _hudModel.AnimateStartPrompt("ControlsPrompt".Localize());
            await _hudModel.AnimateStartPrompt("StartPrompt".Localize());

            SetPause(false);
            OnGameStart?.Invoke();
        }
    }
}
