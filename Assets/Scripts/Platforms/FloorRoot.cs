using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Platforms.Components;
using UnityEngine;
using Zenject;
using Random = UnityEngine.Random;


namespace Platforms
{
public class FloorRoot : MonoBehaviour
{
    // private Boost _boosts // TODO: Add coins and boosts

    private List<GameObject> platforms; // platforms
    private List<GameObject> otherObjects; // etc

    public Action OnFloorPass;

    private void Awake()
    {
        platforms = new List<GameObject>();
        otherObjects = new List<GameObject>();
    }

    private void OnTriggerEnter( Collider other )
    {
        if ( other.TryGetComponent<Player>( out _ ) )
        {
            BreakFloor();
        }
    }

    public void BreakFloor()
    {
        OnFloorPass?.Invoke();
        OnFloorPass = null;
        DeactivateFloor();
    }

    public void AddPlatform( GameObject platform )
    {
        GetComponent<Collider>().enabled = true;
        platforms.Add( platform );
    }

    public void AddObject( GameObject otherObject )
    {
        otherObjects.Add( otherObject );
    }

    public void InstantlyDeactivateFloor()
    {
        DeactivateFloor( true );
    }
    
    private void DeactivateFloor( bool deactivateInstantly = false )
    {
        foreach ( var platform in platforms )
        {
            platform.GetComponent<RotationComponent>().StopRotation();
            platform.layer = 8; // broken platform
            var pRigidbody = platform.GetComponent<Rigidbody>();
            pRigidbody.isKinematic = false;
            
            if ( deactivateInstantly )
            {
                platform.GetComponent<ResetComponent>().Reset();
            }
            else
            {
                pRigidbody.AddRelativeForce(
                    Vector3.left *
                    (10 + Random.Range( 0, 100 )) );
                pRigidbody.AddRelativeTorque(
                    Random.rotation.eulerAngles *
                    (Random.Range( 0, 1 ) > 0.5f ? -1 : 1) );
                StartCoroutine( DoAfterSeconds(
                    () => platform.GetComponent<ResetComponent>().Reset(), 0.5f ) );
            }
        }
        platforms.Clear();
        
        foreach ( var otherObject in otherObjects )
        {
            otherObject.GetComponent<ResetComponent>().Reset();
        }
        otherObjects.Clear();
    }

    private IEnumerator DoAfterSeconds( Action action, float seconds )
    {
        yield return new WaitForSeconds( seconds );
        action.Invoke();
    }
}
}