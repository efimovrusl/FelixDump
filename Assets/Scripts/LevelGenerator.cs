using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Object = UnityEngine.Object;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Space(5), Header("Prefab refs")]
    [SerializeField] private GameObject floorRoot;
    [SerializeField] private GameObject helix;
    [SerializeField] private GameObject platform30; 
    [SerializeField] private GameObject platform45; 
    [SerializeField] private GameObject redPlatform20; 
    [SerializeField] private GameObject redPlatform45;
    
    [Space(5), Header("Object refs")]
    [SerializeField] private Player player;
    [SerializeField] private Transform helixTransform;
    
    [Space(5), Header("Settings")]
    [SerializeField, Range(1, 5)] private int difficulty;
    [SerializeField, Range(10, 100)] private int height;
    
    // Also, the height of cylinder section
    private const float FloorHeight = 1.5f;
    
    
    private void Awake()
   {

        player.onDeath += () =>
        {
            Time.timeScale = 0;
            StartCoroutine(Timer(2, () => SceneManager.LoadScene("MainScene")));
        };
        
        GenerateLevel();
   }

    private void GenerateLevel()
    {
        int pathLeft = 0;
        
        // which of 12 (360/30) sections is for straight-fall path
        int pathSection = 0;
        const int sectionAmount = 12;
        var hasPlatform = new int[sectionAmount];

        for (int i = 0; i < height; i++, pathLeft--)
        {
            var floorRotationDelta = difficulty * Random.Range(-5f, 5f);
            var floorPosition = Vector3.down * i * FloorHeight;

            FloorRoot floor = Instantiate(floorRoot, floorPosition, 
                helixTransform.rotation *
                Quaternion.Euler(0, floorRotationDelta, 0), 
                helixTransform).GetComponent<FloorRoot>();
            Instantiate(helix, floorPosition, helixTransform.rotation, helixTransform);

            for (int k = 0; k < sectionAmount; k++) hasPlatform[k] = 1;

            // defining where non-path hole is
            int nonPathHoleSection = Random.Range(pathSection + 3, 
                pathSection + sectionAmount - 4) % sectionAmount;
            // first section
            hasPlatform[nonPathHoleSection] = 0;
            // second section
            hasPlatform[(nonPathHoleSection + 1) % sectionAmount] = 0;
            // possibly third section
            if (Random.Range(0f, 1f) > 0.666f)
                hasPlatform[(nonPathHoleSection + 2) % sectionAmount] = 0;
            
            if (pathLeft > 0)
            {
                // defining where path-hole is
                hasPlatform[pathSection] = 0;
                hasPlatform[(pathSection + 1) % sectionAmount] = 0;
            }
            else 
            {
                if (Random.Range(0f, 1f) > 0.777f)
                {
                    pathLeft = Random.Range(5, 20);
                    pathSection = Random.Range(0, sectionAmount - 1);
                    Debug.Log("Path section: " + $"{pathSection}");
                }
            }

            for (int j = 0; j < sectionAmount; j++)
            {
                if (hasPlatform[j] == 1)
                {
                    floor.SpawnPlatform(platform30, 
                        Quaternion.Euler(0, 30 * j, 0));   
                }
            }
            
        }
    }

    IEnumerator Timer(float seconds, Action fuck)
    {
        yield return new WaitForSecondsRealtime(seconds);
        Time.timeScale = 1;
        fuck.Invoke();
    }

    
}


