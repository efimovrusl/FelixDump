using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace Managers
{
public class GameManager : MonoBehaviour
{
    [Inject] private SceneLoader sceneLoader;

    private void Awake()
    {
        Application.targetFrameRate = 90;
        
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