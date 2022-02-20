using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


public class FloorRoot : MonoBehaviour
{
    // private Boost _boosts // TODO: Add coins and boosts

    private List<GameObject> platforms; // platforms, walls, etc

    // private LevelGenerator

    public event UnityAction OnFloorPass;
    
    private void Awake()
    {
        platforms = new List<GameObject>(16);
    }

    static int cntr = 0; 
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            OnFloorPass?.Invoke();
            DeactivateFloor();
            Debug.Log($"LevelPassed: {cntr++}");
        }
    }
    
    public void AddPlatform(GameObject platform)
    {
        GetComponent<Collider>().enabled = true;
        platforms.Add(platform);
        platform.layer = 6; // platform
    }

    private void DeactivateFloor()
    {
        GetComponent<Collider>().enabled = false;
        foreach (var platform in platforms)
        {
            platform.SetActive(false);

            // platform.transform.parent = null;
            // platform.layer = 8; // broken platform
            // var pRigidbody = platform.GetComponent<Rigidbody>();
            // pRigidbody.isKinematic = false;
            // pRigidbody.AddRelativeForce(Vector3.left * 100);

            // StartCoroutine(DoAfterSeconds(
            //     () => platform.SetActive(false), 1));
        }
    }

    private IEnumerator DoAfterSeconds(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    } 

}
