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
        Container.BindInterfacesAndSelfTo<UIModelLoading>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIModelAbout>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIModelMenu>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIModelGameOver>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIModelPause>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIModelHud>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIModelPopup>().AsSingle();
        Container.BindInterfacesAndSelfTo<UIModelSettings>().AsSingle();
    }
}
