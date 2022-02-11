using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction onDeath;
    
    
    public void TakeDamage()
    {
        onDeath.Invoke();
    }
}
