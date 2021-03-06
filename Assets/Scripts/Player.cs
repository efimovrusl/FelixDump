using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction onDeath;
    private Rigidbody _rigidbody;
    
    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // doubling default gravity (default = 7.0 m/s^2)
        _rigidbody.AddForce(Physics.gravity * Time.fixedDeltaTime, ForceMode.VelocityChange);
        
        // limiting speed for the player to be able
        // to control helix rotation like he's Neo
        _rigidbody.velocity = Vector3.ClampMagnitude(_rigidbody.velocity, 7);
    }


    public void TakeDamage()
    {
        onDeath?.Invoke();
    }
}
