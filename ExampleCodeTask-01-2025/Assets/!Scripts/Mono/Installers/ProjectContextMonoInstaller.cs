using UnityEngine;
using Zenject;

public class ProjectContextMonoInstaller : MonoInstaller
{
    [SerializeField] private UISystemMediator _uiSystemMediator;

    public override void InstallBindings()
    {
        InstallSystems();
        InstallUI();
    }

    public void InstallSystems()
    {
        Container.BindInterfacesTo<ResourcesSystem>().AsSingle();
        Container.BindInterfacesTo<InputSystem>().AsSingle();
        Container.BindInterfacesTo<UISystem>().
            FromInstance(new UISystem(_uiSystemMediator)).AsSingle();
    }

    public void InstallUI()
    {

    }
}
