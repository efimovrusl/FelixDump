using UnityEngine;

namespace Platforms
{
public class FinishFloor : MonoBehaviour
{
    private void OnTriggerEnter( Collider other )
    {
        if ( other.TryGetComponent( out Player player ) )
        {
            player.TouchFinishPlatform();
        }
    }
}
}