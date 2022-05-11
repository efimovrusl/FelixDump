using System;
using System.Collections;
using UnityEngine;

namespace Platforms.Components
{
    
[DisallowMultipleComponent]
public class RotationComponent : MonoBehaviour
{
    public void StartCyclicRotation(Quaternion deltaRotation, float cyclePeriod) => 
        StartCoroutine(LocalRotationCoroutine(deltaRotation, cyclePeriod));
        
    public void StartRotation(Quaternion deltaRotation, float time) => 
        StartCoroutine(RotateLocally(deltaRotation, time));
    
    public void StopRotation() => StopAllCoroutines();

    private IEnumerator LocalRotationCoroutine(Quaternion deltaRotation, float cycleDuration)
    {
        // TODO: FIX ROTATIONS - one of the coroutines or both, or whatever
        
        if (float.IsPositiveInfinity(cycleDuration)) yield break;
        
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();

        while (true)
        {
            yield return StartCoroutine(RotateLocally(
            deltaRotation, cycleDuration / 2));
            deltaRotation = Quaternion.Inverse(deltaRotation);
        }

    }

    private IEnumerator RotateLocally(Quaternion deltaRotation, float time)
    {
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        Quaternion initialRotation = transform.localRotation;
        Quaternion targetRotation = initialRotation * deltaRotation;
        for (float elapsedTime = 0; elapsedTime < time; )
        {
            elapsedTime += Time.deltaTime; // to fully complete rotation
            transform.localRotation = Quaternion.Slerp(
                initialRotation, targetRotation, elapsedTime / time);
            yield return waitForEndOfFrame;
        }

    }

}
}
