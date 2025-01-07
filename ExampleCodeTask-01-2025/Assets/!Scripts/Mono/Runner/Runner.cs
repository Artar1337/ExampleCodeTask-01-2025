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
    [Inject] private UIModelMenu _menuModel;

    private async void Start()
    {
        await _resourcesSystem.Init();
        await _gameCycleSystem.InitializeGame();
        await _levelSystem.Init(_resourcesSystem, _instantiator);

        await _uiSystem.Show(_menuModel);
    }
}
