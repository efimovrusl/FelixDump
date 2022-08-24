using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Zenject;

namespace Managers
{
public class PerformanceManager : MonoBehaviour
{
    public event Action<int> OnFpsChange;

    // for FPS counting purpose
    private Queue<float> DeltaTimesValues;
    private int FPS => (int)(1f / (DeltaTimesValues.Sum() / DeltaTimesValues.Count));

    private void Awake()
    {
        DeltaTimesValues = new Queue<float>( Enumerable.Repeat( Time.deltaTime, 20 ).ToArray() );
    }

    private void Update()
    {
        DeltaTimesValues.Enqueue( Time.deltaTime );
        DeltaTimesValues.Dequeue();
        OnFpsChange?.Invoke( FPS );
    }
}
}