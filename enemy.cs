using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles enemy movement, firing, and collision detection
public class enemy : MonoBehaviour
{
    [SerializeField]
    private float speed = 4.0f; // Enemy movement speed

    private player _player; // Reference to the player script
    private Animator _anim; // Animator for enemy death animation
    private AudioSource _audioSource; // Audio source for enemy death sound

    [SerializeField]
    private GameObject _laserPrefab; // Prefab for enemy laser

    private float _fireRate = 3.0f; // Delay between laser shots
    private float _canFire = -1; // Time until the enemy can fire again

    void Start()
    {
        // Find and assign the Player script
        _player = GameObject.Find("Player").GetComponent<player>();

        // Get the audio source component
        _audioSource = GetComponent<AudioSource>();

        // Debug warning if player is not found
        if (_player == null)
        {
            Debug.LogError("The Player is Null!");
        }

        // Get the animator component
        _anim = GetComponent<Animator>();

        // Debug warning if animator is not found
        if (_anim == null)
        {
            Debug.LogError("The Animator is Null!");
        }
    }

    void Update()
    {
        // Handle movement
        CalculateMovement();

        // Fire laser if cooldown has passed
        if (Time.time > _canFire)
        {
            // Set a random fire rate
            _fireRate = Random.Range(4f, 7f);
            _canFire = Time.time + _fireRate;

            // Instantiate laser prefab
            GameObject enemyLaser = Instantiate(_laserPrefab, transform.position, Quaternion.identity);

            // Get all laser components and assign them as enemy lasers
            laser[] lasers = enemyLaser.GetComponentsInChildren<laser>();
            for (int i = 0; i < lasers.Length; i++)
            {
                lasers[i].AssignEnemyLaser();
            }
        }
    }

    // Controls downward movement and screen wrap
    void CalculateMovement()
    {
        transform.Translate(Vector3.down * speed * Time.deltaTime);

        // If the enemy goes off-screen, reposition at the top with a random X
        if (transform.position.y < -10)
        {
            float randomX = Random.Range(-9f, 9f);
            transform.position = new Vector3(randomX, 3, 0);
        }
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Collision with the player
        if (other.tag == "Player")
        {
            player player = other.transform.GetComponent<player>();

            if (player != null)
            {
                // Damage the player
                player.Damage();
            }

            // Trigger death animation and sound
            _anim.SetTrigger("OnEnemyDeath");
            speed = 0.0f;
            Destroy(this.gameObject, 1.5f);
            _audioSource.Play();
        }

        // Collision with a laser
        if (other.tag == "Laser")
        {
            laser laser = other.GetComponent<laser>();

            // Ignore if it's an enemy laser
            if (laser != null && laser._isEnemyLaser == true)
            {
                return;
            }

            // Destroy the laser
            Destroy(other.gameObject);

            // Add score to the player
            if (_player != null)
            {
                _player.AddScore(10);
            }

            // Trigger death animation and sound
            _anim.SetTrigger("OnEnemyDeath");
            speed = 0.0f;
            Destroy(GetComponent<Collider2D>());
            Destroy(this.gameObject, 1.5f);
            _audioSource.Play();
        }
    }
}
