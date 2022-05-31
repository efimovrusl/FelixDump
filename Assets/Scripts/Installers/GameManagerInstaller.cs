using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
public class GameManagerInstaller : MonoInstaller
{
    [SerializeField] private GameManager gameManager;
    public override void InstallBindings()
    {
        Container.Bind<GameManager>().FromInstance(gameManager).AsSingle();
    }
}
}