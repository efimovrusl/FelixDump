using Managers;
using UnityEngine;
using Zenject;

public class LevelUIManagerInstaller : MonoInstaller
{
    [SerializeField] private LevelUIManager levelUIManager;
    public override void InstallBindings()
    {
        Container.Bind<LevelUIManager>().FromInstance(levelUIManager).AsSingle();
    }
}