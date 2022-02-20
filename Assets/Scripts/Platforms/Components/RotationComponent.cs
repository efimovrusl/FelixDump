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
        yield break;
        WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
        Quaternion initialRotation = transform.localRotation;
            
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
        Quaternion startingRotation = transform.localRotation;
        Quaternion targetRotation = startingRotation * deltaRotation;
        for (float elapsedTime = 0; elapsedTime < time; )
        {
            elapsedTime += Time.deltaTime; // to fully complete rotation
            transform.localRotation = Quaternion.Slerp(
                startingRotation, targetRotation, elapsedTime / time);
            yield return waitForEndOfFrame;
        }
    }

}
}
