using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script handles the explosion effect and destroys it after a short time
public class explosion : MonoBehaviour
{
    void Start()
    {
        // Destroy the explosion object after 1 second to clean up the scene
        Destroy(this.gameObject, 1.0f);
    }
}
