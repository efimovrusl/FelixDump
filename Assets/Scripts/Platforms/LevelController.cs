using System;
using System.Collections;
using System.Collections.Generic;
using Platforms.Components;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using SystemRandom = System.Random;


namespace Platforms
{
public class LevelController : MonoBehaviour
{
    // ReSharper disable ConvertToLambdaExpression
    [Space( 5 ), Header( "ObjectPooling factories" ), SerializeField]
    private PoolFactory cylinderFactory;

    [SerializeField] private PoolFactory floorRootFactory;
    [SerializeField] private PoolFactory platform30Factory;
    [SerializeField] private PoolFactory redPlatform20Factory;
    [SerializeField] private PoolFactory finishFloorFactory;

    [Space( 5 ), Header( "Object refs" ), SerializeField]
    private Transform helixTransform;

    [Inject] private Player player;

    /// <summary> Range from 0 to 255 </summary>
    public int Difficulty
    {
        set => difficulty = Math.Clamp( value, 0, 255 );
    }

    private int difficulty;

    /// <summary> Range from 10 to 100 </summary>
    public int Height
    {
        set => height = Math.Clamp( value, 10, 100 );
    }

    private int height;

    private const int SectorsAmount = 12;
    private const float FloorGap = 1.5f;
    public float Y( int floorIndex ) => -1 * FloorGap * floorIndex;

    public bool LevelIsPassed { get; private set; }
    public UnityAction OnLevelPass;
    public event Action<int> OnPointsIncrease;

    private List<FloorRoot> floorRoots = new List<FloorRoot>();

    private void Start()
    {
        OnLevelPass += () => LevelIsPassed = true;
        player.OnTouchFinishPlatform += () => OnLevelPass.Invoke();
    }

    public void GenerateLevel( int difficulty, int height )
    {
        this.difficulty = difficulty;
        this.height = height;

        LevelIsPassed = false;

        StartCoroutine( LevelGenerationCoroutine() );
    }

    public void DestroyLevel()
    {
        StopAllCoroutines();
        foreach ( var floorRoot in floorRoots )
            floorRoot.InstantlyDeactivateFloor();
        floorRoots.Clear();
        helixTransform.rotation = Quaternion.identity;
    }

    private IEnumerator LevelGenerationCoroutine()
    {
        int pointsCounter = 0;
        int floorsToGenerate = 7;
        FloorRoot currentFloorRoot;
        var levelStructureGenerator = new LevelStructureGenerator( difficulty, height, SectorsAmount );

        for ( int floorIndex = -2; floorIndex <= height; floorIndex++ )
        {
            if ( floorsToGenerate <= 0 ) yield return new WaitUntil( () => floorsToGenerate > 0 );
            floorsToGenerate--;

            cylinderFactory.NextInstance( helixTransform, Vector3.up * Y( floorIndex ), Quaternion.identity );

            if ( floorIndex < 0 ) continue;
            if ( floorIndex == height )
            {
                currentFloorRoot = floorRootFactory.NextInstance( helixTransform,
                    Vector3.up * Y( floorIndex ),
                    Quaternion.identity ).GetComponent<FloorRoot>();
                var finishFloor = finishFloorFactory.NextInstance( currentFloorRoot.transform, 
                    Vector3.zero, Quaternion.identity );
                currentFloorRoot.AddObject( finishFloor );
                floorRoots.Add( currentFloorRoot );
                continue;
            }

            var floorStructure = levelStructureGenerator.GetFloorStructure( floorIndex );

            currentFloorRoot = floorRootFactory.NextInstance( helixTransform,
                    Vector3.up * Y( floorIndex ),
                    Quaternion.Euler( 0, floorStructure.YRotation, 0 ) )
                .GetComponent<FloorRoot>();
            floorRoots.Add( currentFloorRoot );

            currentFloorRoot.OnFloorPass += () =>
            {
                floorsToGenerate++;
                OnPointsIncrease( ++pointsCounter );
                player.PassFloor();
            };

            // spawning static 30-degree platforms
            foreach ( var platform in floorStructure.Platforms )
            {
                currentFloorRoot.AddPlatform( platform30Factory.NextInstance( currentFloorRoot.transform,
                    Vector3.zero, Quaternion.Euler( 0, (float)platform.Section / SectorsAmount * 360, 0 ) ) );
            }

            // spawning 20-degree red platforms (with spikes)
            foreach ( var platform in floorStructure.RedPlatforms )
            {
                var redPlatform = redPlatform20Factory.NextInstance( currentFloorRoot.transform, Vector3.zero,
                    Quaternion.Euler( 0, (float)platform.StartSection / SectorsAmount * 360 - 5, 0 ) );
                redPlatform.GetComponent<RotationComponent>().StartCyclicRotation(
                    Quaternion.Euler( 0,
                        (float)(platform.EndSection - platform.StartSection) / SectorsAmount * 360 + 10,
                        0 ), platform.RotationHalfPeriod );
                currentFloorRoot.AddPlatform( redPlatform );
            }
        }
    }
}
}