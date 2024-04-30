using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinishLevel : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Entered");
        if (other.gameObject.CompareTag("Player") && other.gameObject.GetComponent<PlayerController>()._collectedArtForLevel)
        {
            //Load the next level
            UnityEngine.SceneManagement.SceneManager.LoadScene(UnityEngine.SceneManagement.SceneManager.GetActiveScene().buildIndex + 1);
            other.gameObject.GetComponent<PlayerController>()._collectedArtForLevel = false;
        }
    }
}
