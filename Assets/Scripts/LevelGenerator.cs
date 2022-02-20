using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;
using Random = UnityEngine.Random;

public class LevelGenerator : MonoBehaviour
{
    [Space(5), Header("ObjectPooling factories")] 
    [SerializeField] private PoolFactory platform30Factory;
    [SerializeField] private PoolFactory redPlatform20Factory;
    [SerializeField] private PoolFactory cylinderFactory;
    [SerializeField] private PoolFactory floorRootFactory;
    
    [Space(5), Header("Object refs")]
    [SerializeField] private Transform helixTransform;
    
    [Space(5), Header("Settings")]
    [SerializeField, Range(1, 5)] private int difficulty;
    [SerializeField, Range(10, 100)] private int height;
    
    
    // Also, the height of cylinder section
    private const float FloorHeight = 1.5f;
    
    // INJECTED AT STARTUP
    [Inject] private Player player;

    
    

    private void Start()
    {
        
        
        player.onDeath += () =>
        {
            SceneManager.LoadScene("MainScene");
        };
        
        
        
        StartCoroutine(GenerateLevel());
   }

    // TODO: Encapsulate generation to outer class
    IEnumerator GenerateLevel()
    {
        int pathLeft = 0;
        
        // which of 12 (360/30) sections is for straight-fall path
        int pathSection = 0;
        const int sectionAmount = 12;
        var hasPlatform = new int[sectionAmount];
        
        int floorsToGenerate = 5;
        float floorRotationDelta = 0;

        
        // Generate 2 cylinders on top of the first floor
        cylinderFactory.GetInstance(helixTransform, GetFloorCenterPosition(-2), Quaternion.identity);
        cylinderFactory.GetInstance(helixTransform, GetFloorCenterPosition(-1), Quaternion.identity);

        for (int i = 0; i < height; i++, pathLeft--)
        {
            yield return new WaitUntil(() =>
            {
                if (floorsToGenerate <= 0) return false;
                
                floorsToGenerate--;
                return true;
            });

            floorRotationDelta += Mathf.Sqrt(difficulty) * Random.Range(-10f, 10f);
            var floorPosition = GetFloorCenterPosition(i);

            FloorRoot floor = floorRootFactory.GetInstance(helixTransform,
                floorPosition, Quaternion.identity).GetComponent<FloorRoot>();
            
            floor.OnFloorPass += () =>
            {
                floorsToGenerate++;
            };

            cylinderFactory.GetInstance(helixTransform, floorPosition, Quaternion.identity);

            
            //       >>>>> LEVEL GENERATION BEGINS HERE <<<<<
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
                }
            }

            for (int j = 0; j < sectionAmount; j++)
            {
                if (hasPlatform[j] == 1)
                {
                    floor.AddPlatform(platform30Factory.GetInstance(floor.transform, 
                        Vector3.zero, Quaternion.Euler(0, 30 * j, 0)));
                }
            }
            
        }
    }
    
    /// <summary>
    /// Gets floor center coordinates based on floorHeight const and floor's index
    /// </summary>
    /// <param name="floorIndex">starts from 0</param>
    /// <returns></returns>
    private Vector3 GetFloorCenterPosition(int floorIndex) =>
        Vector3.down * floorIndex * FloorHeight;

}






