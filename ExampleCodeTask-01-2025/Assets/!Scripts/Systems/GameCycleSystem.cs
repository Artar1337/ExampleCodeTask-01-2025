using Cysharp.Threading.Tasks;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public interface IGameCycleSystem
{
    bool Paused { get; }

    void SetPause(bool paused);
    void StartGame();
    void EndGame();
    UniTask InitializeGame();
}

public class GameCycleSystem : IGameCycleSystem
{
    [Inject] private IPlayerMovementSystem _playerMovementSystem;
    [Inject] private IResourcesSystem _resources;
    [Inject] private IUISystem _uiSystem;
    [Inject] private IGameScoreSystem _scoreSystem;
    [Inject] private UIModelGameOver _gameOverModel;
    [Inject] private UIModelHud _hudModel;

    public bool Paused { get; private set; }

    public void EndGame()
    {
        SetPause(true);
        var setCopy = new HashSet<IUIModel>(_uiSystem.ShownModels);

        foreach (var model in setCopy)
        {
            _uiSystem.Hide(model).Forget();
        }

        _uiSystem.Show(_gameOverModel).Forget();
    }

    public async UniTask InitializeGame()
    {
        SetPause(true);
        // Load player
        var player = await _resources.Instantiate<PlayerMovementMediator>("Player");
        _playerMovementSystem.Init(player);
        // Load 'lobby' level
    }

    public void SetPause(bool paused)
    {
        Paused = paused;
        Physics.simulationMode = paused ?
            SimulationMode.Script : SimulationMode.FixedUpdate;
    }

    public void StartGame()
    {
        SetPause(false);
        _scoreSystem.ResetScore();
        // Hide all ui
        var setCopy = new HashSet<IUIModel>(_uiSystem.ShownModels);
        
        foreach (var model in setCopy)
        {
            _uiSystem.Hide(model).Forget();
        }

        _uiSystem.Show(_hudModel).Forget();
        // Reset player
        _playerMovementSystem.ResetBall();
        // Load 'lobby' level
    }
}
