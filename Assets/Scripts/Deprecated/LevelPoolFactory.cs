using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Factory has pool of pre-instantiated objects
/// for building level at runtime.
///
/// It reuses the oldest objects, keeping them in queue.
///
/// TODO: Somehow attach factory as singleton to each object separately so that wouldn't break DRY rule
/// </summary>
public class LevelPoolFactory : MonoBehaviour
{
    // TODO: Do something with code duplication if it's possible

    [SerializeField] private GameObject 
        platform30, 
        redPlatform20, 
        cylinder;
    
    private const int 
        Platform30Amount = 128, 
        RedPlatform20Amount = 32, 
        CylinderAmount = 8;

    private Queue<GameObject> 
        platform30Queue, 
        redPlatform20Queue, 
        cylinderQueue;

    #region Pool initialization
    private void Awake()
    {
        InitializePoolQueue(ref platform30Queue, Platform30Amount, platform30);
        InitializePoolQueue(ref redPlatform20Queue, RedPlatform20Amount, redPlatform20);
        InitializePoolQueue(ref cylinderQueue, CylinderAmount, cylinder);
    }
    
    private static Queue<GameObject> InitializePoolQueue(
        ref Queue<GameObject> queue, int size, GameObject gameObject)
    {
        queue ??= new Queue<GameObject>();
        for (int i = 0; i < size; i++)
        {
            GameObject obj = Instantiate(gameObject);
            obj.AddComponent<ResetComponent>();
            obj.AddComponent<YRotatingComponent>();
            queue.Enqueue(obj);
        }
        return queue;
    }
    #endregion

    public GameObject GetPlatform30(int floor,
        float minAngle, float? maxAngle = null, float cyclePeriod = 3f) =>
        GetPooledObject(platform30, floor, minAngle, maxAngle, cyclePeriod);
    
    public GameObject GetRedPlatform20(int floor,
        float minAngle, float? maxAngle = null, float cyclePeriod = 3f) =>
        GetPooledObject(redPlatform20, floor, minAngle, maxAngle, cyclePeriod);

    public GameObject GetCylinder(int floor,
        float minAngle, float? maxAngle = null, float cyclePeriod = 3f) =>
        GetPooledObject(cylinder, floor, minAngle, maxAngle, cyclePeriod);

    private GameObject GetPooledObject(GameObject obj, int floor, 
        float minAngle, float? maxAngle = null, float cyclePeriod = 3f)
    {
        throw new NotImplementedException();
        
        return null;
    }
    

    [DisallowMultipleComponent]
    internal class YRotatingComponent : MonoBehaviour
    {
        public void StartRotation(float degrees,
            RotationDirection direction, float cyclePeriod) => 
            StartCoroutine(RotationCoroutine(degrees, direction, cyclePeriod));

            private IEnumerator RotationCoroutine(float degrees, 
            RotationDirection direction, float cycleDuration)
        {
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            Quaternion startingRotation = transform.localRotation;
            Quaternion targetRotation = startingRotation * Quaternion.Euler(0, 
                    (direction == RotationDirection.Clockwise ? 1 : -1) * degrees, 0);

            Quaternion currentTargetRotation = targetRotation;

            while (true)
            {
                yield return StartCoroutine(RotateTo(
                    currentTargetRotation, cycleDuration / 2));
                
                currentTargetRotation = currentTargetRotation == targetRotation ? 
                    startingRotation : targetRotation;
            }
        }

        private IEnumerator RotateTo(Quaternion targetRotation, float time)
        {
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            Quaternion startingRotation = transform.localRotation;
            for (float elapsedTime = 0; elapsedTime < time; )
            {
                elapsedTime += Time.deltaTime; // to fully complete rotation
                transform.localRotation = Quaternion.Slerp(
                    startingRotation, targetRotation, elapsedTime / time);
                yield return waitForEndOfFrame;
            }
        }

        public void StopRotating() => StopAllCoroutines();

        public enum RotationDirection { Clockwise, Counterclockwise }
    }
    
    [DisallowMultipleComponent]
    internal class ResetComponent : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private YRotatingComponent _rotatingComponent;
        
        private void Start() => Reset();

        public void Reset()
        {
            if (!TryGetComponent<Rigidbody>(out _))
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.isKinematic = true;
            _rigidbody.rotation = Quaternion.identity;
            _rigidbody.velocity = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

}

