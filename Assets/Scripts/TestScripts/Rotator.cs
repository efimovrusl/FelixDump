using System;
using System.Collections;
using System.Collections.Generic;
using Platforms.Components;
using Unity.Mathematics;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    private RotationComponent rotationComponent;
    
    private void Awake()
    {
        if (!TryGetComponent<RotationComponent>(out _))
            rotationComponent = gameObject.AddComponent<RotationComponent>();
    }

    private void Start()
    {
        rotationComponent.StartCyclicRotation(Quaternion.Euler(0, 60, 0), 2);
    }
    
    // private IEnumerator Start()
    // {
    // while (true)
    //     {
    //         rotationComponent.StartRotation(Quaternion.Euler(0, 90, 0), 1);
    //         yield return new WaitForSeconds(2);
    //     }
    // }
}
