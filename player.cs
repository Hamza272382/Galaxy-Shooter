using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class player : MonoBehaviour
{
    // Player movement speed
    [SerializeField]
    public float speed = 20.0f;
    // Speed multiplier for speed boost power-up
    public float speedMultiplier = 2.0f;

    // Prefab for the single laser shot
    [SerializeField]
    private GameObject _laserPrefab;
    // Prefab for the triple shot power-up
    [SerializeField]
    private GameObject _tripleShotPrefab;

    // Time delay between shots
    [SerializeField]
    private float _fireRate = 0.15f;
    // Tracks when player can fire next
    private float _canFire = -1f;

    // Player lives
    [SerializeField]
    private int _lives = 3;

    // Reference to the spawn manager script
    private spawn_manager _spawnManager;

    // Flags for power-ups
    private bool _isTripleShotActive = false;
    private bool _isSpeedBoostActive = false;
    private bool _isShieldActive = false;

    // Shield visualizer game object
    [SerializeField]
    private GameObject _shieldVisualizer;

    // Visual effects for damaged engines
    [SerializeField]
    private GameObject _leftEngine, _rightEngine;

    // Player score
    [SerializeField]
    private int score;

    // Reference to the UI manager
    private ui_manager UI_Manager;

    // Audio clips and source for laser sound effect
    [SerializeField]
    private AudioClip _laserSound;
    [SerializeField]
    private AudioSource _laserSource;


    // Called once at the start of the game
    void Start()
    {
        // Set initial position of player
        transform.position = new Vector3(0.78f, -6.26f, 0);

        // Get the Spawn Manager component from Spawn_Manager GameObject
        _spawnManager = GameObject.Find("Spawn_Manager").GetComponent<spawn_manager>();

        // Get the UI Manager component from Canvas GameObject
        UI_Manager = GameObject.Find("Canvas").GetComponent<ui_manager>();

        // Get the AudioSource component attached to this player GameObject
        _laserSource = GetComponent<AudioSource>();

        // Error checking for components
        if (_spawnManager == null)
        {
            Debug.LogError("The Spawn Manager is NULL!.");
        }
        if (UI_Manager == null)
        {
            Debug.LogError("The UI Manager is NULL!.");
        }
        if (_laserSource == null)
        {
            Debug.LogError("The Audio Source on the player is NULL!.");
        }
        else
        {
            // Assign laser sound clip to audio source
            _laserSource.clip = _laserSound;
        }
    }

    // Called once per frame
    void Update()
    {
        // Handle player movement
        calculateMovement();

        // Fire laser if left mouse button clicked or spacebar pressed and cooldown allows
        if (Input.GetMouseButtonDown(0) && Time.time > _canFire)
        {
            laser();
        }
        if (Input.GetKeyDown(KeyCode.Space) && Time.time > _canFire)
        {
            laser();
        }
    }

    // Calculates and applies player movement based on input and power-ups
    void calculateMovement()
    {
        // Normal movement
        if(_isSpeedBoostActive == false)
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * horizontalInput * speed * Time.deltaTime);
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * verticalInput * speed * Time.deltaTime);
        }
        else // Speed boost active, increase speed multiplier
        {
            float horizontalInput = Input.GetAxis("Horizontal");
            transform.Translate(Vector3.right * horizontalInput * speed * speedMultiplier * Time.deltaTime);
            float verticalInput = Input.GetAxis("Vertical");
            transform.Translate(Vector3.up * verticalInput * speed * speedMultiplier * Time.deltaTime);
        }

        // Clamp vertical movement to max height and min depth
        if (transform.position.y >= 1.5)
        {
            transform.position = new Vector3(transform.position.x, 1.5f, 0);
        }
        else if (transform.position.y <= -8.3)
        {
            transform.position = new Vector3(transform.position.x, -8.3f, 0);
        }

        // Wrap horizontal position to the other side when player goes out of bounds
        if (transform.position.x >= 9.2)
        {
            transform.position = new Vector3(-9.2f, transform.position.y, 0);
        }
        else if (transform.position.x <= -9.2)
        {
            transform.position = new Vector3(9.2f, transform.position.y, 0);
        }
    }

    // Handles laser firing logic, instantiates laser or triple shot prefab
    void laser()
    {
        // Set next allowed fire time
        _canFire = Time.time + _fireRate;

        // Fire triple shot if power-up active
        if (_isTripleShotActive == true)
        {
            Instantiate(_tripleShotPrefab, transform.position + new Vector3(-3.29f, -0.75f, 0) , Quaternion.identity);
        }
        else // Fire normal single laser
        {
            Instantiate(_laserPrefab, transform.position + new Vector3(0, 1.05f, 0), Quaternion.identity);
        }

        // Play laser firing sound
        _laserSource.Play();
    }

    // Called when player takes damage
    public void Damage()
    {
        // If shield is active, absorb the damage and disable shield
        if(_isShieldActive == true)
        {
            _isShieldActive = false;
            _shieldVisualizer.SetActive(false);
            return;
        }

        // Reduce player lives by 1
        _lives--;

        // Show damaged engine effects depending on remaining lives
        if (_lives == 2 )
        {
            _leftEngine.SetActive(true);
        }
        else if (_lives == 1)
        {
            _rightEngine.SetActive(true);
        }

        // Update UI to show current lives
        UI_Manager.UpdateLives(_lives);

        // If no lives left, handle player death
        if(_lives < 1)
        {
            _spawnManager.OnPlayerDeath();
            UI_Manager.BestScore();
            Destroy(this.gameObject);
        }
    }

    // Activate triple shot power-up
    public void TripleShotActive()
    {
        _isTripleShotActive = true;
        StartCoroutine(TripleShotPowerDownRoutine());
    }

    // Coroutine to disable triple shot after 10 seconds
    IEnumerator TripleShotPowerDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _isTripleShotActive = false;
    }

    // Activate speed boost power-up
    public void SpeedBoostActive()
    {
        _isSpeedBoostActive = true;
        StartCoroutine(SpeedBoostDownRoutine());
    }

    // Coroutine to disable speed boost after 10 seconds
    IEnumerator SpeedBoostDownRoutine()
    {
        yield return new WaitForSeconds(10.0f);
        _isSpeedBoostActive = false;
    }

    // Activate shield power-up and enable shield visualizer
    public void ShieldActive()
    {
        _isShieldActive = true;
        _shieldVisualizer.SetActive(true);
    }

    // Add points to player score and update UI
    public void AddScore(int points)
    {
        score += points;
        UI_Manager.UpdateScore();
    }

}
