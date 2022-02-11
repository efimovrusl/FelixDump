using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Game object is being DELETED when game starts
/// so it can only be seen in editor
/// </summary>
public class EditorOnly : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject);
    }
    
}
