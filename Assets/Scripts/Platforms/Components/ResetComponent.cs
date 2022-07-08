using System.ComponentModel;
using UnityEngine;

namespace Platforms.Components
{
[DisallowMultipleComponent]
public class ResetComponent : MonoBehaviour
{
    private Rigidbody rigbody;
    
    // becomes parent of the object after Resetting
    public Transform parentForInactiveState;


    public void Reset()
    {
        GetComponent<RotationComponent>().StopRotation();
        transform.parent = parentForInactiveState;
        if (!TryGetComponent<Rigidbody>(out _))
            rigbody = gameObject.AddComponent<Rigidbody>();

        rigbody.isKinematic = true;
        gameObject.layer = 6; // platforms' layer
        gameObject.SetActive(false);
    }
}
}

