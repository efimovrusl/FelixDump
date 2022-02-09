using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EditorOnly : MonoBehaviour
{
    private void Start()
    {
        Destroy(gameObject);
    }
    
}
