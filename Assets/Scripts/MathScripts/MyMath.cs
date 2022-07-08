using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace MathScripts
{
public static class MyMath
{
    public class WeightedRandomBag<T>  {

        private struct Entry {
            public double AccumulatedWeight;
            public T Item;
        }

        private readonly List<Entry> entries = new List<Entry>();
        private double accumulatedWeight;
        private readonly Random rand = new Random();

        public WeightedRandomBag(IList<T> items, IList<float> probabilities)
        {
            if (items.Count != probabilities.Count) throw new Exception("Invalid amount of probabilities");
            for (int i = 0; i < items.Count; i++) AddEntry(items[i], probabilities[i]);
        }

        public WeightedRandomBag(IList<(T item, float probability)> itemProbabilityTuples) : this(
            itemProbabilityTuples.Select(item => item.item).ToList(),
            itemProbabilityTuples.Select(item => item.probability).ToList()) { }


        public void AddEntry(T item, double weight) {
            accumulatedWeight += weight;
            entries.Add(new Entry { Item = item, AccumulatedWeight = accumulatedWeight });
        }

        public T GetRandom() {
            double r = rand.NextDouble() * accumulatedWeight;
            foreach (var entry in entries.Where(entry => entry.AccumulatedWeight >= r)) return entry.Item;
            
            //should only happen when there are no entries
            throw new Exception("No entries to return.");
        }
    }

    public static void CloserToZero(ref int value)
    {
        if (value > 0) value--;
        if (value < 0) value++;
    }

    public static int SignOfNumber(ref int value)
    {
        return value >= 0 ? 1 : -1;
    }
    
}
}
