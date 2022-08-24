using UnityEngine;

namespace Platforms
{
public class FinishFloor : MonoBehaviour
{
    private void OnCollisionEnter( Collision collision )
    {
        if ( collision.collider.TryGetComponent( out Player player ) )
        {
            player.TouchFinishPlatform();
        }
    }
}
}