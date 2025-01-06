using Cysharp.Threading.Tasks;
using UnityEngine;
using Zenject;

public class Runner : MonoBehaviour
{
    [Inject] private IUISystem _uiSystem;
    [Inject] private IResourcesSystem _resourcesSystem;
    [Inject] private IGameCycleSystem _gameCycleSystem;
    [Inject] private UIModelMenu _menuModel;

    private async void Start()
    {
        await _resourcesSystem.Init();
        await _gameCycleSystem.InitializeGame();

        await _uiSystem.Show(_menuModel);
    }
}
