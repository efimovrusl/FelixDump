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
            Debug.Log($"LevelPassed: {cntr++}");
        }
    }
    
    public void AddPlatform(GameObject platform)
    {
        platforms.Add(platform);
        // Instantiate(prefab, transform.position, 
        // transform.rotation * rotation, transform));
    }

    public void DestroyFloor()
    {
        GetComponent<Collider>().enabled = false;
        foreach (var platform in platforms)
        {
            platform.transform.parent = null;
            var pRigidbody = platform.GetComponent<Rigidbody>();
            pRigidbody.isKinematic = false;
            pRigidbody.AddRelativeForce(Vector3.left * 100);

            StartCoroutine(DoAfterSeconds(
                () => platform.SetActive(false), 1));
        }
        gameObject.SetActive(false);
    }

    private IEnumerator DoAfterSeconds(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    } 

}
