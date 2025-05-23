using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles asteroid behavior including rotation and collision with lasers
public class asteroid : MonoBehaviour
{
    [SerializeField]
    private float _rotatespeed = 3.0f; // Speed at which the asteroid rotates

    [SerializeField]
    private GameObject _explosionPrefab; // Explosion effect prefab when asteroid is destroyed

    private spawn_manager _spawnManager; // Reference to the spawn manager to start spawning enemies

    void Start()
    {
        // Find the Spawn_Manager object in the scene and get the spawn_manager component
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<spawn_manager>();
    }

    void Update()
    {
        // Continuously rotate the asteroid around the Z-axis
        transform.Rotate(Vector3.forward * _rotatespeed * Time.deltaTime);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the object colliding with the asteroid is tagged as "Laser"
        if (other.tag == "Laser")
        {
            // Instantiate the explosion effect at the asteroid's position
            Instantiate(_explosionPrefab, transform.position, Quaternion.identity);

            // Destroy the laser that hit the asteroid
            Destroy(other.gameObject);

            // Start spawning enemies or other objects
            _spawnManager.StartSpawning();

            // Destroy the asteroid shortly after the collision to allow the explosion effect to play
            Destroy(this.gameObject, 0.25f);
        }
    }
}
