using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
public class PerformanceManagerInstaller : MonoInstaller
{
    [SerializeField] private PerformanceManager performanceManager;

    public override void InstallBindings()
    {
        Container.Bind<PerformanceManager>().FromInstance( performanceManager ).AsSingle();
    }
}
}