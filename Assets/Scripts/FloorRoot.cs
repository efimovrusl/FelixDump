using System;
using System.Collections;
using System.Collections.Generic;
using Platforms.Components;
using UnityEngine;
using UnityEngine.Events;
using Zenject;
using Random = UnityEngine.Random;


public class FloorRoot : MonoBehaviour
{
    // private Boost _boosts // TODO: Add coins and boosts

    private List<GameObject> platforms; // platforms, walls, etc

    public event UnityAction OnFloorPass;
    
    private void Awake()
    {
        platforms = new List<GameObject>(20);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out _))
        {
            OnFloorPass?.Invoke();
            OnFloorPass = null;
            DeactivateFloor();
        }
    }
    
    public void AddPlatform(GameObject platform)
    {
        GetComponent<Collider>().enabled = true;
        platforms.Add(platform);
    }

    private void DeactivateFloor()
    {
        foreach (var platform in platforms)
        {
            platform.GetComponent<RotationComponent>().StopRotation();
            platform.layer = 8; // broken platform
            var pRigidbody = platform.GetComponent<Rigidbody>();
            pRigidbody.isKinematic = false;
            pRigidbody.AddRelativeForce(
                Vector3.left * 
                (10 + Random.Range(0, 100)));
            pRigidbody.AddRelativeTorque(
                Random.rotation.eulerAngles * 
                (Random.Range(0, 1) > 0.5f ? -1 : 1));
            StartCoroutine(DoAfterSeconds(
                () => platform.SetActive(false), 0.5f));
        }
        platforms.Clear();
    }

    private IEnumerator DoAfterSeconds(Action action, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        action.Invoke();
    } 

}
