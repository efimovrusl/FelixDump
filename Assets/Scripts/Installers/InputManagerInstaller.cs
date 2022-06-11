using Managers;
using UnityEngine;
using Zenject;

public class InputManagerInstaller : MonoInstaller
{
    [SerializeField] private InputManager inputManager;
    
    public override void InstallBindings()
    {
        Container.Bind<InputManager>().FromInstance(inputManager).AsSingle();
    }
}