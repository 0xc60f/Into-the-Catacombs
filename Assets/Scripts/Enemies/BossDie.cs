using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDie : MonoBehaviour
{
    // Start is called before the first frame update
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            Debug.Log("bruh");
            if (GameObject.Find("Grunt").GetComponent<Boss>().numLives > 1)
            {
                GameObject.Find("Grunt").GetComponent<AudioSource>().Play();
                GameObject.Find("Grunt").GetComponent<Boss>().numLives--;
            }
            else
            {
                GameObject.Find("Grunt").SetActive(false);
                GameObject.Find("FinalCutscene").SetActive(true);
            }
        }
    }
}
