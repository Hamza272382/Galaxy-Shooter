using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script manages game states such as game over, pause, resume, and quitting the game
public class game_manager : MonoBehaviour
{
    [SerializeField]
    private bool _isgameOver; // Tracks whether the game is over

    [SerializeField]
    private GameObject _pauseMenuPannal; // Reference to the pause menu UI panel

    [SerializeField]
    private Animator _pauseAnimator; // Animator controlling pause menu animations

    public void Start()
    {
        // Find the pause menu GameObject and get its Animator component
        _pauseAnimator = GameObject.Find("PauseManue").GetComponent<Animator>();

        // Set animator update mode to unscaled time so animations run while the game is paused
        _pauseAnimator.updateMode = AnimatorUpdateMode.UnscaledTime;
    }

    private void Update()
    {
        // Reload the game scene if 'R' is pressed and the game is over
        if (Input.GetKeyDown(KeyCode.R) && _isgameOver == true)
        {
            SceneManager.LoadScene(1);
        }

        // Quit the application if 'Escape' key is pressed
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }

        // Pause the game if 'P' key is pressed
        if (Input.GetKeyDown(KeyCode.P))
        {
            // Show the pause menu panel
            _pauseMenuPannal.SetActive(true);

            // Trigger the pause animation
            _pauseAnimator.SetBool("isPaused", true);

            // Freeze game time
            Time.timeScale = 0;
        }
    }

    // Called to mark the game as over
    public void GameOver()
    {
        _isgameOver = true;
    }

    // Resumes the game from pause state
    public void ResumeGame()
    {
        // Hide the pause menu panel
        _pauseMenuPannal.SetActive(false);

        // Resume game time
        Time.timeScale = 1;
    }
}
