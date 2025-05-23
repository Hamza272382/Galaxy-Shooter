using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawn_manager : MonoBehaviour
{
    // Enemy prefab to spawn
    [SerializeField]
    private GameObject _enemyPrefab;

    // Container object to keep spawned enemies organized in hierarchy
    [SerializeField]
    private GameObject _enemyContainer;

    // Array of power-up prefabs to spawn randomly
    [SerializeField]
    private GameObject[] powerups;

    // Flag to control stopping the spawning routines
    private bool _stopSpawning = false;


    // Public method to start spawning enemies and power-ups
    public void StartSpawning()
    {
        StartCoroutine(SpawnRoutine());
        StartCoroutine(SpawnPowerUpRoutine());
    }

    // Coroutine to spawn enemies repeatedly with a delay
    IEnumerator SpawnRoutine()
    {
        // Initial wait before starting enemy spawn
        yield return new WaitForSeconds(3.0f);

        // Spawn enemies while spawning is not stopped
        while (_stopSpawning == false)
        {
            // Random horizontal position between -9 and 9, spawn at y=3
            Vector3 posToSpawn = new Vector3(Random.Range(-9f, 9f), 3, 0);

            // Instantiate enemy prefab at random position
            GameObject newEnemy = Instantiate(_enemyPrefab, posToSpawn, Quaternion.identity);

            // Set enemy parent to keep hierarchy clean
            newEnemy.transform.parent = _enemyContainer.transform;

            // Wait for 5 seconds before spawning next enemy
            yield return new WaitForSeconds(5.0f);
        }
    }

    // Coroutine to spawn power-ups repeatedly with random intervals
    IEnumerator SpawnPowerUpRoutine()
    {
        // Initial wait before starting power-up spawn
        yield return new WaitForSeconds(3.0f);

        // Spawn power-ups while spawning is not stopped
        while (_stopSpawning == false)
        {
            // Random horizontal position between -9 and 9, spawn at y=3
            Vector3 possToSpawn = new Vector3(Random.Range(-9f, 9f), 3, 0);

            // Pick a random index between 0 and 2 for power-ups array
            int random = Random.Range(0, 3);

            // Instantiate a random power-up prefab at the random position
            Instantiate(powerups[random], possToSpawn, Quaternion.identity);

            // Wait for a random time between 3 and 8 seconds before next spawn
            yield return new WaitForSeconds(Random.Range(3,8));
        }
    }

    // Call this method when the player dies to stop spawning enemies and power-ups
    public void OnPlayerDeath()
    {
        _stopSpawning = true;
    }
}
