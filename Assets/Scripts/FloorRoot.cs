using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Zenject;


public class FloorRoot : MonoBehaviour
{
    // private Boost _boosts // TODO: Add coins and boosts

    private List<GameObject> _platforms; // platforms, walls, etc

    // private LevelGenerator

    public event UnityAction OnFloorPass;
    
    private void Awake()
    {
        _platforms = new List<GameObject>(16);
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
        _platforms.Add(Instantiate(prefab, transform.position, 
            transform.rotation * rotation, transform));
    }

    public void DestroyFloor()
    {
        GetComponent<Collider>().enabled = false;
        foreach (var platform in _platforms)
        {
            
            platform.transform.parent = null;
            var pRigidbody = platform.AddComponent<Rigidbody>();
            pRigidbody.AddRelativeForce(Vector3.left * 100);

            StartCoroutine(DoAfterSeconds(
                () => Destroy(platform), 1));



        }
        // Destroy(this.gameObject);
    }

    private IEnumerator DoAfterSeconds(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    } 

}
