using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[SelectionBase, DisallowMultipleComponent]
public abstract class Platform : MonoBehaviour
{
    [SerializeField] protected float angle;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.collider.TryGetComponent<Player>(out var player))
        {
            player.GetComponent<Rigidbody>().velocity = -Physics.gravity * 0.7f;
        }
    }
}
