using System;
using UnityEngine;
using Zenject;

namespace Managers
{
public class GameManager : MonoBehaviour
{
    [Inject] private SceneLoader sceneLoader;

    private void Awake()
    {
        LoadMenuScene();
    }

    public void LoadMenuScene()
    {
        sceneLoader.LoadMenuScene();
    }

    public void LoadLevelScene()
    {
        sceneLoader.LoadLevelScene();
    }

    public void QuitApplication()
    {
        Application.Quit();
    }
}
}
