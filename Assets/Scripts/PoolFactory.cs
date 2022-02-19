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
public class PoolFactory : MonoBehaviour
{
    [SerializeField] private GameObject prefab;
    [SerializeField, Range(1, 1024)] private int poolSize = 128;
    
    // TODO: Test if "new()" works
    private readonly Queue<GameObject> poolQueue = new Queue<GameObject>();

    #region Pool initialization
    private void Awake()
    {
        InitializePoolQueue();
    }
    
    private void InitializePoolQueue()
    {
        for (int i = 0; i < poolSize; i++)
        {
            GameObject obj = Instantiate(prefab);
            obj.AddComponent<ResetComponent>();
            obj.AddComponent<RotationComponent>();
            poolQueue.Enqueue(obj);
        }
    }
    #endregion

    /// <summary> Position and rotation args are treated as LOCAL </summary>
    public GameObject GetInstance(Transform parentTransform, Vector3 position, Quaternion rotation) =>
        GetInstance(parentTransform, position, rotation, rotation, 
            float.PositiveInfinity);
        
    /// <summary> Position and rotation args are treated as LOCAL </summary>
    public GameObject GetInstance(Transform parentTransform, Vector3 position, 
        Quaternion initialRotation, Quaternion endRotation, float cyclePeriod = 3f)
    {
        GameObject instance = poolQueue.Dequeue();
        instance.transform.parent = parentTransform;
        poolQueue.Enqueue(instance);
        instance.SetActive(true);
        instance.transform.localPosition = position;
        instance.transform.localRotation = initialRotation;
        instance.GetComponent<RotationComponent>().StartCyclicRotation(endRotation, cyclePeriod);
        return instance;
    }

    [DisallowMultipleComponent]
    internal class ResetComponent : MonoBehaviour
    {
        private Rigidbody _rigidbody;
        private RotationComponent _rotatingComponent;

        // Resets gameobject on adding ResetComponent or scene loading
        // TODO: Check if Reset is called automatically on Awake & delete if so
        private void Awake() => Reset(); 

        public void Reset()
        {
            transform.parent = null;
            if (!TryGetComponent<Rigidbody>(out _))
                _rigidbody = gameObject.AddComponent<Rigidbody>();
            _rigidbody.useGravity = true;
            _rigidbody.velocity = Vector3.zero;
            _rigidbody.isKinematic = true;
            gameObject.transform.localRotation = Quaternion.identity;
            gameObject.transform.localPosition = Vector3.zero;
            gameObject.SetActive(false);
        }
    }

}

