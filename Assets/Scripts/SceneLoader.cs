using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Remoting.Messaging;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

/// <summary>
/// Encapsulation of scene loading process
/// </summary>
public class SceneLoader : MonoBehaviour
{
    [SerializeField] private Object menuScene;
    [SerializeField] private List<Object> menuDecoratorScenes;

    [SerializeField] private Object levelScene;
    [SerializeField] private List<Object> levelDecoratorScenes;

    /// <summary>
    /// 1) UNLOADS LEVEL scene and all its' decorator scenes
    /// 2) LOADS all decorating scenes for MENU scene
    /// 3) LOADS MENU scene 
    /// </summary>
    public void LoadMenuScene()
    {
        StartCoroutine(QueryCoroutines(new[]
        {
            UnloadLevelScene(), LoadSceneAfterDecoratorScenes(
                menuScene.name, menuDecoratorScenes.Select(scene => scene.name))
        }));
    }

    /// <summary>
    /// 1) UNLOADS MENU scene and all its' decorator scenes
    /// 2) LOADS all decorating scenes for LEVEL scene
    /// 3) LOADS LEVEL scene 
    /// </summary>
    public void LoadLevelScene()
    {
        StartCoroutine(QueryCoroutines(new[]
        {
            UnloadMenuScene(), LoadSceneAfterDecoratorScenes(
                levelScene.name, levelDecoratorScenes.Select(scene => scene.name))
        }));
    }

    private IEnumerator UnloadMenuScene()
    {
        yield return StartCoroutine(UnloadSceneBeforeDecoratorScenes(
            menuScene.name, menuDecoratorScenes.Select(scene => scene.name)));
    }

    private IEnumerator UnloadLevelScene()
    {
        yield return StartCoroutine(UnloadSceneBeforeDecoratorScenes(
            levelScene.name, levelDecoratorScenes.Select(scene => scene.name)));
    }
    
    /// <summary>Loads decorator scenes and only then loads main scene</summary>
    /// <param name="scene">Main scene, which must be loaded after all scenes with decorator contexts</param>
    /// <param name="decoratorScenes">Scenes with decorator contexts, which must be loaded earlier</param>
    /// <returns></returns>
    private IEnumerator LoadSceneAfterDecoratorScenes(string scene, IEnumerable<string> decoratorScenes)
    {
        yield return StartCoroutine(QueryCoroutines(new[]
        {
            _LoadScenesAsync(decoratorScenes),
            _LoadSceneAsync(scene)
        }));
        SceneManager.SetActiveScene(SceneManager.GetSceneByName(scene));
    }
    
    /// <summary>Unloads main scene and only then unloads decorator scenes</summary>
    /// <param name="scene">Main scene, which must be unloaded first to avoid null refs</param>
    /// <param name="decoratorScenes">Scenes with decorator contexts, which get unloaded later</param>
    /// <returns></returns>
    private IEnumerator UnloadSceneBeforeDecoratorScenes(string scene, IEnumerable<string> decoratorScenes)
    {
        yield return StartCoroutine(QueryCoroutines(new[]
        {
            _UnloadSceneAsync(scene),
            _UnloadScenesAsync(decoratorScenes)
        }));
    }

    // Load single scene
    private IEnumerator _LoadSceneAsync(string sceneName)
    {   // avoiding code duplication
        yield return StartCoroutine(_LoadScenesAsync(new[] {sceneName}));
    }
    
    // Load multiple scenes
    private IEnumerator _LoadScenesAsync(IEnumerable<string> scenes)
    {
        var sceneLoadingOperations = scenes.Select(scene => SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
        yield return new WaitUntil(() => sceneLoadingOperations.All(asyncOperation => asyncOperation.isDone));
    }
    
    // Unload single scene
    private IEnumerator _UnloadSceneAsync(string sceneName)
    {   // avoiding code duplication
        yield return StartCoroutine(_UnloadScenesAsync(new List<string> {sceneName}));
    }

    // Unload multiple scenes
    private IEnumerator _UnloadScenesAsync(IEnumerable<string> scenes)
    {
        var sceneUnloadingOperations = scenes.Select(SceneManager.UnloadSceneAsync);
        yield return new WaitUntil(() => sceneUnloadingOperations.All(asyncOperation => asyncOperation.isDone));
    }

    private IEnumerator QueryCoroutines(IEnumerable<IEnumerator> coroutines)
    {
        yield return coroutines.Select(StartCoroutine).GetEnumerator();
    }

}
