using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HelpScreen : MonoBehaviour
{


    // Update is called once per frame
    void Update()
    {
        //If any key pressed
        if (Input.anyKey)
        {
            //Load the main menu
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
    }
}
