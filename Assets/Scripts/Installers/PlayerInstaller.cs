using UnityEngine;
using Zenject;

namespace Installers
{
public class PlayerInstaller : MonoInstaller
{
    [SerializeField] private Player player;
    
    public override void InstallBindings()
    {
        Container.Bind<Player>().FromInstance(player).AsSingle();
    }
}
}