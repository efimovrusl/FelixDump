using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PlayerFollowingCamera : MonoBehaviour
{
    [SerializeField] private Player _player;

    private void Update()
    {
        
        if (_player.transform.position.y < transform.position.y)
        {
            // camera moves down when player reaches any point lower
            transform.position = new Vector3(
                transform.position.x, 
                _player.transform.position.y, 
                transform.position.z);
        }
    }
}
