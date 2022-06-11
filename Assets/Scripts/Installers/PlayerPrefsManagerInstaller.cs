using Managers;
using UnityEngine;
using Zenject;

namespace Installers
{
public class PlayerPrefsManagerInstaller : MonoInstaller
{
    [SerializeField] private PlayerPrefsManager playerPrefsManager; 
    public override void InstallBindings()
    {
        Container.Bind<PlayerPrefsManager>().FromInstance(playerPrefsManager).AsSingle();
    }
}
}