using System.Collections.Generic;
using Platforms.Components;
using UnityEngine;

/// <summary>
/// Factory has pool of pre-instantiated objects
/// for building level at runtime.
///
/// It reuses the oldest objects, keeping them in queue.
/// </summary>
public class PoolFactory : MonoBehaviour
{
    // private static int counter = 0;
    
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
        poolQueue.Enqueue(instance);
        instance.GetComponent<ResetComponent>().Reset();
        instance.SetActive(true);
        // instance.gameObject.name = $"{prefab.name} {counter++}";
        instance.transform.parent = parentTransform;
        instance.transform.localPosition = position;
        instance.transform.localRotation = initialRotation;
        instance.GetComponent<RotationComponent>().StartCyclicRotation(endRotation, cyclePeriod);
        return instance;
    }

    

}

