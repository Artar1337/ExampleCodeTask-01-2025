using Logic.Systems;
using UI.Models;
using UnityEngine;
using Zenject;

namespace Mono.Installers
{
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
            Container.BindInterfacesTo<PlayerMovementSystem>().AsSingle();
            Container.BindInterfacesTo<GameCycleSystem>().AsSingle();
            Container.BindInterfacesTo<GameScoreSystem>().AsSingle();
            Container.BindInterfacesTo<LavaSystem>().AsSingle();
            Container.BindInterfacesTo<AudioSystem>().AsSingle();
            Container.BindInterfacesTo<SettingsSystem>().AsSingle();
            Container.BindInterfacesTo<LevelSystem>().AsSingle();
            Container.BindInterfacesTo<PlatformCollisionSystem>().AsSingle();
            Container.BindInterfacesTo<EscapePressHandler>().AsSingle();
            Container.BindInterfacesTo<UISystem>().
                FromInstance(new UISystem(_uiSystemMediator)).AsSingle();
            Container.Bind<AudioPlayer>().AsSingle().NonLazy();
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
}
