using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the laser behavior for both player and enemy lasers
public class laser : MonoBehaviour
{
    [SerializeField]
    private float speed = 10.0f;  // Speed at which the laser moves

    public bool _isEnemyLaser = false;  // Flag to determine if this laser belongs to an enemy

    void Start()
    {
        // No initialization needed here currently
    }

    void Update()
    {
        // Move the laser depending on whether it is an enemy laser or player's
        if (_isEnemyLaser == false)
        {
            MoveUp();  // Player laser moves up
        }
        else
        {
            MoveDown();  // Enemy laser moves down
        }
    }

    // Moves the laser upward and destroys it when it leaves the screen
    void MoveUp()
    {
        transform.Translate(Vector3.up * speed * Time.deltaTime);

        // If laser moves off the top of the screen, destroy it (and its parent if it has one)
        if (transform.position.y > 2.7f)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    // Moves the laser downward and destroys it when it leaves the screen
    void MoveDown()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // If laser moves off the bottom of the screen, destroy it (and its parent if it has one)
        if (transform.position.y < -8.7)
        {
            if (transform.parent != null)
            {
                Destroy(transform.parent.gameObject);
            }
            Destroy(this.gameObject);
        }
    }

    // Mark this laser as an enemy laser
    public void AssignEnemyLaser()
    {
        _isEnemyLaser = true;
    }

    // Detect collision with player and apply damage if this is an enemy laser
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == "Player" && _isEnemyLaser == true)
        {
            player player = other.transform.GetComponent<player>();

            if (player != null)
            {
                player.Damage();
            }
        }
    }
}
