using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ui_manager : MonoBehaviour
{
    // UI elements to show score and best score
    [SerializeField]
    private Text _scoreText, bestText;

    // UI Image to show player's lives
    [SerializeField]
    private Image _livesImg;

    // Array of sprites to represent lives count visually
    [SerializeField]
    private Sprite[] _lives;

    // UI Text for Game Over and Restart messages
    [SerializeField]
    private Text _gameover;
    [SerializeField]
    private Text _restart;

    // Reference to game_manager script
    private game_manager _gameManager;

    // Player's current score and best score
    public int score, bestScore;

    // Initialization
    void Start()
    {
        // Initialize score text to zero
        _scoreText.text = "Score: " + 0;

        // Load the saved best score (high score) from PlayerPrefs
        bestScore = PlayerPrefs.GetInt("HighScore", 0);
        bestText.text = "Best: " + bestScore;

        // Hide Game Over and Restart text at start
        _gameover.gameObject.SetActive(false);
        _restart.gameObject.SetActive(false);

        // Find the GameManager in the scene and get the game_manager component
        _gameManager = GameObject.Find("GameManager").GetComponent<game_manager>();

        // Check if the GameManager was found correctly
        if (_gameManager == null)
        {
            Debug.LogError("GameManager is Null!");
        }
    }

    // Update the score and display it on UI
    public void UpdateScore()
    {
        score += 10;
        _scoreText.text = "Score: " + score;
    }

    // Check and update best score if current score exceeds it
    public void BestScore()
    {
        if (score > bestScore)
        {
            bestScore = score;
            PlayerPrefs.SetInt("HighScore", bestScore);
            bestText.text = "Best: " + bestScore;
        }
    }

    // Update the lives image according to current lives count
    public void UpdateLives(int currentLives)
    {
        _livesImg.sprite = _lives[currentLives];

        // Trigger game over sequence if no lives left
        if (currentLives == 0)
        {
            GameOverSequense();
        }
    }

    // Handles the game over state UI and logic
    public void GameOverSequense()
    {
        _gameManager.GameOver();
        _gameover.gameObject.SetActive(true);
        _restart.gameObject.SetActive(true);
        StartCoroutine(GameOverFlickerRoutine());
    }

    // Coroutine to make the "GAME OVER!" text flicker repeatedly
    IEnumerator GameOverFlickerRoutine()
    {
        while (true)
        {
            _gameover.text = "GAME OVER!";
            yield return new WaitForSeconds(0.5f);
            _gameover.text = "";
            yield return new WaitForSeconds(0.5f);
        }
    }

    // Resume gameplay by calling ResumeGame on GameManager
    public void ResumePlay()
    {
        game_manager gm = GameObject.Find("GameManager").GetComponent<game_manager>();
        gm.ResumeGame();
    }

    // Load the main menu scene (assumed to be scene index 0)
    public void BackToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
