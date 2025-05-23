using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class powerup : MonoBehaviour
{
    // Speed at which the power-up moves downward
    [SerializeField]
    private float speed = 3.0f;

    // Identifier for which power-up type this is (0 = Triple Shot, 1 = Speed Boost, 2 = Shield)
    [SerializeField]
    private int PowerupID;

    // Audio clip to play when power-up is collected
    [SerializeField]
    private AudioClip _clip;

    // Update is called once per frame
    void Update()
    {
        // Move the power-up downwards every frame based on speed and frame time
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // Destroy power-up if it moves below y = -9 to clean up off-screen objects
        if (transform.position.y < -9)
        {
            Destroy(this.gameObject);
        }
    }

    // Called when this object's collider enters a trigger collider
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding is tagged as Player
        if(other.tag == "Player")
        {
            // Get the player component from the collided object
            player player = other.transform.GetComponent<player>();

            // Play power-up collection sound at current position
            AudioSource.PlayClipAtPoint(_clip, transform.position);

            // If player component found, activate power-up effect based on PowerupID
            if (player != null)
            {
                switch (PowerupID)
                {
                    case 0:
                        player.TripleShotActive();
                        break;
                    case 1:
                        player.SpeedBoostActive();
                        break;
                    case 2:
                        player.ShieldActive();
                        break;
                    default:
                        Debug.Log("Default");
                        break;
                }
            }

            // Destroy the power-up object after giving the power-up to the player
            Destroy(this.gameObject);
        }
    }
}
