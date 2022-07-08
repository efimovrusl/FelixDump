using System;
using System.Collections;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using ModestTree;
using Platforms.Components;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using static MathScripts.MyMath;
using Random = UnityEngine.Random;
using SystemRandom = System.Random;

namespace Platforms
{
public class LevelController : MonoBehaviour
{
    // ReSharper disable ConvertToLambdaExpression
    [Space(5), Header("ObjectPooling factories")] 

    [SerializeField] private PoolFactory cylinderFactory;
    [SerializeField] private PoolFactory floorRootFactory;
    [SerializeField] private PoolFactory platform30Factory;
    [SerializeField] private PoolFactory redPlatform20Factory;

    [Space(5), Header("Object refs")] 
        
    [SerializeField] private Transform helixTransform;

    [Inject] private Player player;

    /// <summary> Range from 0 to 255 </summary>
    public int Difficulty { set => difficulty = Math.Clamp(value, 0, 255); }
    private int difficulty;

    /// <summary> Range from 10 to 100 </summary>
    public int Height { set => height = Math.Clamp(value, 10, 100); }
    private int height;
        
    public bool LevelIsPassed { get; private set; }
    public UnityAction OnLevelPass;

    public void Generate(int difficulty, int height)
    {
        this.difficulty = difficulty;
        this.height = height;
            
        LevelIsPassed = false;
        OnLevelPass += () => LevelIsPassed = true;
        OnLevelPass += OnLevelPass = null;

        StartCoroutine(GeneratorCoroutine());
    }

    private IEnumerator GeneratorCoroutine()
    {
        int floorCounter = 0;
        int floorsToGenerate = 4;
        FloorRoot currentFloorRoot = null;
        FloorTuner floorTuner = new FloorTuner(difficulty, height);

        for (int i = 0; i < height; i++)
        {
            yield return new WaitUntil(() => floorsToGenerate > 0);
            floorsToGenerate--;
            
            var currentFloor = new Floor(floorCounter++, RotateFloor, InitPlatform30, InitRedPlatform20);
            
            currentFloorRoot = floorRootFactory.NextInstance(
                helixTransform, helixTransform.position + Vector3.up * currentFloor.Y, Quaternion.Euler(Vector3.zero))
                .GetComponent<FloorRoot>();
            
            currentFloorRoot.OnFloorPass += player.TakeDamage;
            
            floorTuner.TuneNext(currentFloor);
            
            currentFloor.Generate();
        }

        void RotateFloor(float yRotation)
        {
            currentFloorRoot.transform.localRotation *= Quaternion.Euler(Vector3.up * yRotation);
        }
        
        void InitPlatform30(float y, float yRotation)
        {
            currentFloorRoot.AddPlatform(platform30Factory.NextInstance(
                    helixTransform, helixTransform.position + Vector3.up * y, Quaternion.Euler(0, yRotation, 0)));
        }
        
        void InitRedPlatform20(float y, float yRotation, float clockwiseEndRotationAngle, float period)
        {
            var platform = redPlatform20Factory.NextInstance(
                    helixTransform, helixTransform.position + Vector3.up * y, Quaternion.Euler(0, yRotation, 0));
            platform.GetComponent<RotationComponent>()
                .StartCyclicRotation(Quaternion.Euler(0, clockwiseEndRotationAngle - 20, 0), period);
            currentFloorRoot.AddPlatform(platform);
        }
    }

    private class FloorTuner
    {
        private readonly int difficulty;
        private readonly int height;
        private List<Floor> floors = new List<Floor>();

        private int straightHoleLengthLeft = 0;
        private int straightHoleIndex;

        private int rotationDirection = 0;
        
        
        public FloorTuner(int difficulty, int height)
        {
            this.difficulty = difficulty;
            this.height = height;
        }

        public void TuneNext(Floor floor)
        {
            floors.Add(floor);
            
            if (straightHoleLengthLeft > 0)
            {
                straightHoleLengthLeft--;
                floor.AddHole(straightHoleIndex);
                floor.AddHole(straightHoleIndex + 1);
            }
            else if (Random.value > 0.8f)
            {
                straightHoleLengthLeft = Random.Range(4, 16);
                straightHoleIndex = Random.Range(0, Floor.SectorsAmount);
            }

            if (straightHoleLengthLeft > 0)
            {
                var bag = new WeightedRandomBag<int>(new (int, float)[]
                {   // (amountOfHoles, chanceWeight)
                    (0, 200),
                    (1, 400),
                    (2, 200),
                    (3, 100)
                });
                
                floor.AddRandomHoles(bag.GetRandom());
            }

            if (rotationDirection == 0)
            {
                rotationDirection = Random.Range(-10, 10);
            }
            else
            {
                floor.rotateFloor(SignOfNumber(ref rotationDirection) * 
                                  (5 + 10 * MathF.Sqrt(difficulty) / MathF.Sqrt(difficulty)));
                CloserToZero(ref rotationDirection);
            }
        }
        
    }
    

    private class Floor
    {
        public const int SectorsAmount = 12;
        private const float FloorGap = 1.5f;
        public float Y => -1 * FloorGap * floorNumber;
        public int FloorNumber => floorNumber;
        private float RedPlatformRandomPeriod => Random.Range(2f, 4f);

        private bool isGenerated = false;

        private readonly bool[] hasSafePlatform = new bool[SectorsAmount];
        private readonly List<(int startSector, int endSector, float period)> redPlatforms = new List<(int, int, float)>();

        public delegate void RotateFloor(float yRotation);
        public delegate void InitPlatform30(float y, float yRotation);
        public delegate void InitRedPlatform20(float y, float yRotation, float clockwiseEndRotationAngle, float period);

        private readonly int floorNumber;
        public readonly RotateFloor rotateFloor;
        private readonly InitPlatform30 initPlatform30;
        private readonly InitRedPlatform20 initRedPlatform20;

        public Floor(int floorNumber, 
            RotateFloor rotateFloor, InitPlatform30 initPlatform30, InitRedPlatform20 initRedPlatform20)
        {
            this.floorNumber = floorNumber;
            this.rotateFloor = rotateFloor;
            this.initPlatform30 = initPlatform30;
            this.initRedPlatform20 = initRedPlatform20;

            for (int i = 0; i < SectorsAmount; i++) AddPlatform(i);
        }

        public void AddPlatform(int sector)
        {
            if (isGenerated) throw new Exception("Can't change floor - it's already generated!");
            
            hasSafePlatform[_clampIndex(sector, SectorsAmount)] = true;
        }

        public void AddRedPlatform(int startSector, int endSector, float period)
        {
            if (isGenerated) throw new Exception("Can't add redPlatform - floor is already generated!");
            
            for (int i = startSector; i <= endSector; i++) AddHole(i);
            redPlatforms.Add((startSector, endSector, period));
        }
        
        public void AddHole(int sector)
        {
            if (isGenerated) throw new Exception("Can't change floor - it's already generated!");
            
            hasSafePlatform[_clampIndex(sector, SectorsAmount)] = false;
        }

        public void AddRandomHoles(int holesAmount)
        {
            if (isGenerated) throw new Exception("Can't add holes - floor is already generated!");
            if (hasSafePlatform.All(_ => false)) return;

            var sectionsAllowedToRemove = Enumerable.Range(0, SectorsAmount).ToList();
            foreach (var redPlatform in redPlatforms)
                sectionsAllowedToRemove.RemoveAll(index => 
                    index + 1 >= _clampIndex(redPlatform.Item1, SectorsAmount) && 
                    index - 1 <= _clampIndex(redPlatform.Item2, SectorsAmount));

            
            // calculating amount of platforms which could be removed to add holes
            int amountOfSpaceForHoles = sectionsAllowedToRemove.Count - (holesAmount - 1);
            int[] holesWidths = Enumerable.Repeat(1, holesAmount).ToArray();
            
            // making holes as big as it's possible but still fitting in given space
            amountOfSpaceForHoles -= holesAmount;
            for (int i = 0; i < holesAmount && amountOfSpaceForHoles > 0; i++)
            {
                for (int j = 0; j < 3 && amountOfSpaceForHoles > 0; j++)
                {
                    holesWidths[i]++;
                    amountOfSpaceForHoles--;
                }
            }
            
            // randomly mixing holes with different width
            SystemRandom random = new SystemRandom();
            holesWidths = holesWidths.OrderBy(_ => random.Next()).ToArray();

            // adding random spaces around each hole
            int[] spacesAroundHoles = Enumerable.Repeat(1, holesAmount + 1).ToArray();
            for (int i = 0; i < amountOfSpaceForHoles; i++)
            {
                spacesAroundHoles[Random.Range(0, holesAmount)]++;
            }

            // finding first sector's (in sequence of sectors) index
            int firstIndex = SectorsAmount - 1;
            for (int i = 0; i < SectorsAmount; i++, firstIndex--)
                if (sectionsAllowedToRemove.Contains(_clampIndex(firstIndex, SectorsAmount))
                    && !sectionsAllowedToRemove.Contains(_clampIndex(firstIndex - 1, SectorsAmount))) break;
            
            for (int i = 0, currenSector = firstIndex; i < holesAmount; i++)
            {
                if (holesWidths[i] >= 3)
                {
                    AddRedPlatform(currenSector, currenSector + holesWidths[i], RedPlatformRandomPeriod);
                    currenSector += holesWidths[i];
                }
                else
                {
                    for (int j = 0; j < holesWidths[i]; j++) AddHole(currenSector++);
                }
                for (int j = 0; j < spacesAroundHoles[i]; j++) currenSector++;
            }
        }

        public void Generate()
        {
            if (isGenerated) throw new Exception("Can't generate floor twice!");
            isGenerated = true;

            for (int i = 0; i < SectorsAmount; i++)
            {
                if (hasSafePlatform[i]) 
                    initPlatform30(Y, 360f / SectorsAmount * i);
            }

            foreach (var redPlatform in redPlatforms)
            {
                initRedPlatform20(Y, 
                    redPlatform.startSector * 360f / SectorsAmount,
                    redPlatform.endSector * 360f / SectorsAmount, 
                    redPlatform.period);
            }
        }

        private int _clampIndex(int index, int range) => (index % range + range) % range;
    }
}
}