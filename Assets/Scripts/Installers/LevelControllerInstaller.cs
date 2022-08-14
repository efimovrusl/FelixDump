using Platforms;
using UnityEngine;
using Zenject;

namespace Installers
{
public class LevelControllerInstaller : MonoInstaller
{
    [SerializeField] private LevelController levelController;
    
    public override void InstallBindings()
    {
        Container.Bind<LevelController>().FromInstance(levelController).AsSingle();
    }
}
}
