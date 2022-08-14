using System.Collections;
using System.Collections.Generic;
using System.Linq;
using MathScripts;
using Platforms;
using UnityEngine;
using Random = System.Random;

// using UnityEngine;

public class LevelStructureGenerator
{
    private const float StraightHoleChance = 0.8f;
    private const int StraightHoleMinLength = 4;
    private int StraightHoleMaxLength => floorsAmount / 3;

    private readonly int sectorsAmount;
    private readonly int levelID;
    private readonly int floorsAmount;

    private readonly Random rand;
    private readonly List<FloorStructure> floorStructures;
    private readonly List<List<FloorElements>> levelStructureArray;

    private enum FloorElements
    {
        Hole = 0,
        Platform = 1,
        RedPlatform = 2
    }

    public LevelStructureGenerator( int levelID, int floorsAmount, int sectorsAmount = 12 )
    {
        rand = new Random( this.levelID );
        this.floorsAmount = floorsAmount;
        this.sectorsAmount = sectorsAmount;
        levelStructureArray = GetLevelBasicStructureArray();
        floorStructures = new List<FloorStructure>();
    }

    public FloorStructure GetFloorStructure( int floorIndex )
    {
        FloorStructure floor = new FloorStructure( floorIndex );

        if ( floorIndex >= floorsAmount ) return new FloorStructure( floorIndex );

        // finding static platforms
        for ( int i = 0; i < sectorsAmount; i++ )
        {
            if ( levelStructureArray[floorIndex][i] == FloorElements.Platform )
                floor.Platforms.Add( new FloorStructure.Platform( i ) );
        }

        // finding red platform
        if ( levelStructureArray[floorIndex].Contains( FloorElements.RedPlatform ) )
        {
            int redPlatformSectorsAmount =
                levelStructureArray[floorIndex].FindAll( el => el == FloorElements.RedPlatform ).Count;

            int firstRPIndex = 0; // first redPlatform's index 
            while ( levelStructureArray[floorIndex][MyMath.FracIndex( firstRPIndex, sectorsAmount )] ==
                    FloorElements.RedPlatform ) firstRPIndex++;
            while ( levelStructureArray[floorIndex][MyMath.FracIndex( firstRPIndex, sectorsAmount )] !=
                    FloorElements.RedPlatform ) firstRPIndex++;

            var redPlatformSectors = Enumerable.Range( firstRPIndex, redPlatformSectorsAmount ).ToList();

            floor.RedPlatforms.Add( new FloorStructure.RedPlatform( redPlatformSectors[0], redPlatformSectors[^1] ) );
        }

        return floor;
    }

    private List<List<FloorElements>> GetLevelBasicStructureArray()
    {
        List<List<FloorElements>> staticPlatforms =
            Enumerable.Range( 0, floorsAmount ).Select( _ =>
                Enumerable.Repeat( FloorElements.Platform, sectorsAmount ).ToList() ).ToList();

        int straightFallHoleLeft = 0;
        int straightFallHoleSector = 0;
        for ( int floorIndex = 0; floorIndex < floorsAmount; floorIndex++ )
        {
            if ( straightFallHoleLeft <= 0 && floorIndex > 0 )
            {
                if ( rand.NextDouble() > StraightHoleChance )
                {
                    straightFallHoleLeft = rand.Next( StraightHoleMinLength, StraightHoleMaxLength );
                    straightFallHoleSector = rand.Next( 0, sectorsAmount );
                }
            }
            
            var bagOfHolesToGenerateOnFloor = new MyMath.WeightedRandomBag<int>( new (int, float)[]
            {
                (1, 100), (2, 100), (3, 100), (4, 100) // (amountOfHoles, chanceWeight)
            }, rand );

            // defining "red platform" sectors 
            if ( straightFallHoleLeft > 0 )
            {
                // 2 sectors is quite enough
                staticPlatforms[floorIndex][straightFallHoleSector] = FloorElements.Hole;
                staticPlatforms[floorIndex][MyMath.FracIndex( straightFallHoleSector + 1, sectorsAmount )] =
                    FloorElements.Hole;

                // adding chance for 0 additive holes if there's hole with a red platform
                bagOfHolesToGenerateOnFloor.AddEntry( 0, 100 );
            }

            var consecutiveSequenceOfIndexes = new List<int>();

            if ( staticPlatforms[floorIndex].Contains( FloorElements.Hole ) )
            {
                for ( int i = straightFallHoleSector + 2;; i++ )
                {
                    if ( staticPlatforms[floorIndex][MyMath.FracIndex( i + 1, sectorsAmount )] ==
                         FloorElements.Hole ) break;
                    consecutiveSequenceOfIndexes.Add( MyMath.FracIndex( i, sectorsAmount ) );
                }
            }
            else
            {
                if ( floorIndex > 0 )
                {
                    consecutiveSequenceOfIndexes.AddRange( Enumerable.Range( 0, sectorsAmount ) );
                }
                else
                {
                    // 9th sector must have a platform if it's a first floor, because it's under the player 
                    consecutiveSequenceOfIndexes.AddRange( Enumerable.Range( 10, sectorsAmount - 1 ) );
                }
            }

            var holes = MyMath.GetConsecutiveSubsequenceOfIndexes( consecutiveSequenceOfIndexes,
                bagOfHolesToGenerateOnFloor.GetRandom(), rand );

            foreach ( var hole in holes )
            {
                if ( holes.Length >= 2 )
                {
                    staticPlatforms[floorIndex][MyMath.FracIndex( hole, sectorsAmount )] = FloorElements.RedPlatform;
                }
                else
                {
                    staticPlatforms[floorIndex][MyMath.FracIndex( hole, sectorsAmount )] = FloorElements.Hole;
                }
            }

            straightFallHoleLeft--;
        }

        return staticPlatforms;
    }


    public struct FloorStructure
    {
        private readonly int index;
        public readonly List<Platform> Platforms;
        public readonly List<RedPlatform> RedPlatforms;
        public readonly float YRotation;


        public FloorStructure( int index ) : this()
        {
            this.index = index;
            Platforms = new List<Platform>();
            RedPlatforms = new List<RedPlatform>();
        }

        public struct Platform
        {
            public readonly int Section;

            public Platform( int section )
            {
                Section = section;
            }
        }

        public struct RedPlatform
        {
            public readonly int StartSection;
            public readonly int EndSection;
            public readonly float RotationHalfPeriod;

            public RedPlatform( int startSection, int endSection, float rotationHalfPeriod = 1 )
            {
                StartSection = startSection;
                EndSection = endSection;
                RotationHalfPeriod = rotationHalfPeriod;
            }
        }
    }
}