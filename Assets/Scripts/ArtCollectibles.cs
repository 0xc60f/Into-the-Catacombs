using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtCollectibles : MonoBehaviour
{
    public AudioClip collectSound;
    
   
    void OnTriggerEnter2D(Collider2D other)
    {
      PlayerController controller = other.GetComponent<PlayerController>();
        if (controller != null)
        {
            controller.addArt(1);
              Destroy(gameObject);
            //a value of 1 would pass the first level, and so on so forth.
        //    controller.PlayCollectSound();
             // broken sound effect
          
            
          
        
        }
    }
}
