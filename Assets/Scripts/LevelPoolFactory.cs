using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Factory has pool of pre-instantiated objects
/// for building level at runtime.
///
/// It reuses the oldest objects, keeping them in queue.
/// </summary>
public class LevelPoolFactory : MonoBehaviour
{
    // TODO: Do something with code duplication if it's possible

    [SerializeField] private GameObject platform30;
    [SerializeField] private GameObject redPlatform20;
    [SerializeField] private GameObject cylinder;
    
    private const int _platform30Amount = 128;
    private Queue<GameObject> _platform30Queue;

    private const int _redPlatform20Amount = 32;
    private Queue<GameObject> _redPlatform20Queue;

    private const int _cylinderAmount = 8;
    private Queue<GameObject> _cylinderQueue;

    private void Awake()
    {
        #region Pool initialization

        _platform30Queue = new Queue<GameObject>(_platform30Amount);
        for (int i = 0; i < _platform30Amount; i++)
        {
            var platform = Instantiate(platform30);
            platform.AddComponent<ResetComponent>();
            _platform30Queue.Enqueue(platform);
        }
        
        _redPlatform20Queue = new Queue<GameObject>(_redPlatform20Amount);
        for (int i = 0; i < _redPlatform20Amount; i++)
        {
            var platform = Instantiate(redPlatform20);
            platform.AddComponent<ResetComponent>();
            _redPlatform20Queue.Enqueue(platform);
        }
        
        _cylinderQueue = new Queue<GameObject>(_cylinderAmount);
        for (int i = 0; i < _platform30Amount; i++)
        {
            var platform = Instantiate(platform30);
            platform.AddComponent<ResetComponent>();
            _platform30Queue.Enqueue(platform);
        }
        #endregion
    }

    public GameObject GetStaticObject(int floor, float initRotation)
    {
        return GetRotatingObject(floor, initRotation, initRotation);
    }

    public GameObject GetRotatingObject(int floor, 
        float minAngle, float maxAngle, float cyclePeriod = 3f)
    {
        return null;
    }

    [DisallowMultipleComponent]
    class YRotatingComponent : MonoBehaviour
    {
        private Stack<Coroutine> _startedCoroutines;

        private void Awake()
        {
            _startedCoroutines = new Stack<Coroutine>();
        }

        public void StartRotation(float degrees,
            RotationDirection direction, float cyclePeriod)
        {
            _startedCoroutines.Push(StartCoroutine(
                RotationCoroutine(degrees, direction, cyclePeriod)));
        }

        private IEnumerator RotationCoroutine(float degrees, 
            RotationDirection direction, float cyclePeriod)
        {
            WaitForEndOfFrame waitForEndOfFrame = new WaitForEndOfFrame();
            while (true)
            {
                
                
                yield return waitForEndOfFrame;
            }
            
            
        } 
        
        public void StopRotations()
        {
            while (true)
            {
                var coroutine = _startedCoroutines.Pop();
                if (coroutine == null) break;
                StopCoroutine(coroutine);
            }
        }
        
        public enum RotationDirection
        {
            /// <summary> По часовой стрелке </summary>
            Clockwise,
            /// <summary> Против часовой стрелки </summary>
            Counterclockwise
        }    
    }
    
    [DisallowMultipleComponent]
    class ResetComponent : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private RotatingComponent _rotatingComponent;
        private void Start()
        {
            Reset();
        }

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

