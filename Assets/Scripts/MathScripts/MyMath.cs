using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = System.Random;

namespace MathScripts
{
public static class MyMath
{
    public class WeightedRandomBag<T>
    {
        private struct Entry
        {
            public double AccumulatedWeight;
            public T Item;
        }

        private readonly List<Entry> entries = new List<Entry>();
        private double accumulatedWeight;
        private readonly Random rand;

        public WeightedRandomBag( IList<T> items, IList<float> probabilities, Random rand = null )
        {
            this.rand = rand;
            if ( items.Count != probabilities.Count ) throw new Exception( "Invalid amount of probabilities" );
            for ( int i = 0; i < items.Count; i++ ) AddEntry( items[i], probabilities[i] );
        }

        public WeightedRandomBag( IList<( T item, float probability )> itemProbabilityTuples, Random rand = null ) :
            this(
                itemProbabilityTuples.Select( item => item.item ).ToList(),
                itemProbabilityTuples.Select( item => item.probability ).ToList(), rand )
        {
        }

        public void AddEntry( T item, double weight )
        {
            accumulatedWeight += weight;
            entries.Add( new Entry { Item = item, AccumulatedWeight = accumulatedWeight } );
        }

        public void AddEntries( IList<(T item, float probability)> itemProbabilityTuples )
        {
        }

        public T GetRandom()
        {
            double r = rand.NextDouble() * accumulatedWeight;
            foreach ( var entry in entries.Where( entry => entry.AccumulatedWeight >= r ) ) return entry.Item;

            //should only happen when there are no entries
            throw new Exception( "No entries to return." );
        }
    }

    public static void CloserToZero( ref int value )
    {
        if ( value > 0 ) value--;
        if ( value < 0 ) value++;
    }

    public static int SignOfNumber( ref int value ) => value >= 0 ? 1 : -1;

    public static int FracIndex( int index, int range ) => (index % range + range) % range;

    public static int[] GetConsecutiveSubsequenceOfIndexes( IList<int> sequenceOfConsecutiveIndexes,
        int sizeOfSubsequence, Random random )
    {
        int size = sequenceOfConsecutiveIndexes.Count();
        if ( size < sizeOfSubsequence ) sizeOfSubsequence = size;

        if ( sizeOfSubsequence <= 0 ) return Array.Empty<int>();

        int choices = size - (sizeOfSubsequence - 1);
        int firstElement = random.Next( 0, choices - 1 );
        int[] consecutiveSubsequence = new int[sizeOfSubsequence];

        for ( int i = 0; i < sizeOfSubsequence; i++ )
            consecutiveSubsequence[i] = sequenceOfConsecutiveIndexes[firstElement + i];


        return consecutiveSubsequence;
    }
}
}