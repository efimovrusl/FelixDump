using Platforms;
using UnityEngine;

[SelectionBase, DisallowMultipleComponent]
public abstract class Platform : MonoBehaviour
{
    [SerializeField] protected float angle;

    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.collider.TryGetComponent<Player>( out var player ) )
        {
            player.TouchPlatform( GetComponentInParent<FloorRoot>() );
        }
    }
}