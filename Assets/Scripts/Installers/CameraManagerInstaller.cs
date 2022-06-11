using MyCamera;
using UnityEngine;
using Zenject;

namespace Installers
{
public class CameraManagerInstaller : MonoInstaller
{
    [SerializeField] private CameraManager cameraManager;
    public override void InstallBindings()
    {
        Container.Bind<CameraManager>().FromInstance(cameraManager).AsSingle();
    }
}
}