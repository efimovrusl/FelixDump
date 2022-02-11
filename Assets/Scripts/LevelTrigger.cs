using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class LevelTrigger : MonoBehaviour
{
    public event UnityAction OnFloorPass;
    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<Player>(out var player))
        {
            OnFloorPass?.Invoke();
        }
    }
}
