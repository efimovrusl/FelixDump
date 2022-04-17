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

    private void Start() => Reset();

    public void Reset()
    {
        StopAllCoroutines();
        transform.parent = parentForInactiveState;
        Debug.Log(gameObject.transform.parent);
        if (!TryGetComponent<Rigidbody>(out _))
            rigbody = gameObject.AddComponent<Rigidbody>();
        rigbody.isKinematic = true;
        gameObject.layer = 6; // platforms' layer
        gameObject.SetActive(false);
    }
}
}

