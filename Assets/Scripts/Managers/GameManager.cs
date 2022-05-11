using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;      
                                        
public class GameManager : MonoBehaviour
{                                       
    private void Awake()                
    {                                   
        SceneManager.LoadScene("UI", LoadSceneMode.Additive);
    }
}
                             