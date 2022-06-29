using System.Collections;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

namespace Managers
{
/// <summary>
/// Reusable Scene Manager, which allows asynchronous scenes' loading and unloading
/// TODO: Add loading-bar scene for even smoother transitions between levels
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
        // var sceneLoadingOperations = scenes.Select(scene => SceneManager.LoadSceneAsync(scene, LoadSceneMode.Additive));
        var sceneLoadingOperations = new List<AsyncOperation>();
        foreach (var scene in scenes)
            if (_TryLoadSceneAsync(scene, out var loadingOperation))
                sceneLoadingOperations.Add(loadingOperation);
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
        // var sceneUnloadingOperations = scenes.Select(SceneManager.UnloadSceneAsync); 
        var sceneUnloadingOperations = new List<AsyncOperation>();
        foreach (var scene in scenes)
            if (_TryUnloadSceneAsync(scene, out var unloadingOperation))
                sceneUnloadingOperations.Add(unloadingOperation);
        yield return new WaitUntil(() => sceneUnloadingOperations.All(asyncOperation => asyncOperation.isDone));
    }

    private bool _TryLoadSceneAsync(string sceneName, out AsyncOperation loadingOperation)
    {
        var sceneIsLoaded = false;
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            sceneIsLoaded = sceneName.Equals(SceneManager.GetSceneAt(i).name);
            if (sceneIsLoaded) break;
        }

        if (!sceneIsLoaded)
        {
            loadingOperation = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);
            return true;
        }

        loadingOperation = null;
        return false;
    }
    
    private bool _TryUnloadSceneAsync(string sceneName, out AsyncOperation unloadingOperation)
    {
        for (var i = 0; i < SceneManager.sceneCount; i++)
        {
            if (!sceneName.Equals(SceneManager.GetSceneAt(i).name)) continue;
            unloadingOperation = SceneManager.UnloadSceneAsync(sceneName);
            return true;
        }
        unloadingOperation = null;
        return false;
    }

    private IEnumerator QueryCoroutines(IEnumerable<IEnumerator> coroutines)
    {
        yield return coroutines.Select(StartCoroutine).GetEnumerator();
    }

}
}
