using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;

public class LevelGenerator : MonoBehaviour
{
    [SerializeField] private GameObject platform30; 
    [SerializeField] private GameObject platform45; 
    [SerializeField] private GameObject redPlatform22; 
    [SerializeField] private GameObject redPlatform45;
    [SerializeField] private GameObject helix;

    [SerializeField] private Player _player;

    private Queue<Floor> _floors;

    private void Awake()
   {
       _floors = new Queue<Floor>(10);

        _player.onDeath += () =>
        {
            Time.timeScale = 0;
            StartCoroutine(Timer(2, () => SceneManager.LoadScene("MainScene")));
        };

    }
    

    IEnumerator Timer(float seconds, Action fuck)
    {
        yield return new WaitForSecondsRealtime(2);
        Time.timeScale = 1;
        fuck.Invoke();
    }

}

class Floor
{
    // private Boost _boosts // TODO: Add coins and boosts

    private List<GameObject> _blocks; // platforms, walls, etc
    private Transform _parentTransform;
    private readonly float _positionY;
    
    public Floor(Transform parentTransform, float floorPositionY)
    {
        _parentTransform = parentTransform;
        _positionY = floorPositionY;
        _blocks = new List<GameObject>(16);
    }


    
    
    
    private void SpawnPlatform(GameObject prefab, Quaternion rotation)
    {
        _blocks.Add(Object.Instantiate(prefab, 
            new Vector3(0, _positionY, 0), rotation));
    }
    
    // private void 
}

