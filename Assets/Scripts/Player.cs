using Platforms;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction OnDeath;
    public event UnityAction OnTouchFinishPlatform;
    private Rigidbody rigbody;

    private int floorsPassedWithoutTouch = 0;
    private bool CanBreakAnyPlatform => floorsPassedWithoutTouch > 2;

    private void Awake()
    {
        rigbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // doubling default gravity (default = 7.0 m/s^2)
        rigbody.AddForce( Physics.gravity * Time.fixedDeltaTime, ForceMode.VelocityChange );

        // limiting speed of free-fall for player
        rigbody.velocity = Vector3.ClampMagnitude( rigbody.velocity, 7 );
    }

    public void TouchPlatform( FloorRoot floorRoot )
    {
        if ( CanBreakAnyPlatform )
        {
            floorRoot.BreakFloor();
        }

        Bounce();
    }

    public void Bounce()
    {
        GetComponent<Rigidbody>().velocity = -Physics.gravity * 0.7f;
        floorsPassedWithoutTouch = 0;
    }

    public void PassFloor()
    {
        floorsPassedWithoutTouch++;
    }

    public void TakeDamage( FloorRoot floorRoot )
    {
        if ( CanBreakAnyPlatform )
        {
            TouchPlatform( floorRoot );
        }
        else
        {
            OnDeath?.Invoke();
            rigbody.velocity = Vector3.zero;
        }
    }

    public void TouchFinishPlatform()
    {
        OnTouchFinishPlatform?.Invoke();
    }

    public void TeleportTo( Vector3 initialPlayerPosition )
    {
        transform.position = initialPlayerPosition;
    }
}