using UnityEngine;
using Zenject;

namespace Installers
{
    public class SceneInstaller : MonoInstaller
    {
        [SerializeField] private Player player;
        [SerializeField] private LevelPoolFactory levelPoolFactory;
        public override void InstallBindings()
        {
            Container.Bind<Player>().FromInstance(player).AsSingle();
            Container.Bind<LevelPoolFactory>().FromInstance(levelPoolFactory).AsSingle();
        }
    }
}