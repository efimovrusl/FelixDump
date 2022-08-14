using Platforms;
using UnityEngine;
using UnityEngine.Events;

public class Player : MonoBehaviour
{
    public event UnityAction OnDeath;
    public event UnityAction OnTouchFinishPlatform;
    private Rigidbody _rigidbody;

    private int floorsPassedWithoutTouch = 0;
    private bool CanBreakAnyPlatform => floorsPassedWithoutTouch > 2;

    private void Awake()
    {
        _rigidbody = GetComponent<Rigidbody>();
    }

    private void FixedUpdate()
    {
        // doubling default gravity (default = 7.0 m/s^2)
        _rigidbody.AddForce( Physics.gravity * Time.fixedDeltaTime, ForceMode.VelocityChange );

        // limiting speed of free-fall for player
        _rigidbody.velocity = Vector3.ClampMagnitude( _rigidbody.velocity, 7 );
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
            _rigidbody.velocity = Vector3.zero;
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