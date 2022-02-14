using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


public class FloorRoot : MonoBehaviour
{
    // private Boost _boosts // TODO: Add coins and boosts

    public List<GameObject> platforms; // platforms, walls, etc
    
    // private LevelGenerator

    public event UnityAction OnFloorPass;
    
    private void Awake()
    {
        platforms = new List<GameObject>(16);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            OnFloorPass?.Invoke();
        }
    }
    
    public void SpawnPlatform(GameObject prefab, Quaternion rotation)
    {
        platforms.Add(Instantiate(prefab, 
            transform.position, rotation, transform));
    }
    
}
