using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

// This script handles the main menu functionality
public class main_menu : MonoBehaviour
{
    // This method is called when the player chooses to start the game
    public void LoadGame()
    {
        // Load the scene with index 1 (typically the main game scene)
        SceneManager.LoadScene(1);
    }
}
