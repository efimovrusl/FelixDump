using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;
using Zenject;

public class PlayerFollowingCamera : MonoBehaviour
{
    [Inject] private Player player;

    private void Update()
    {
        if ( player.transform.position.y < transform.position.y ||
             player.transform.position.y > transform.position.y + 1.5f )
        {
            // camera moves down when player reaches any point lower
            transform.position = new Vector3(
                transform.position.x,
                player.transform.position.y,
                transform.position.z );
        }
    }
}