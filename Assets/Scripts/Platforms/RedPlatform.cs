using UnityEngine;

public abstract class RedPlatform : Platform
{
    
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.TryGetComponent<Player>(out var player))
        {
            player.TakeDamage();
        }
    }
}
