using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArtCollectibles : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter2D(Collider2D other)
    {
       PlayerController controller = other.gameObject.GetComponent<PlayerController>();
       if(controller!=null){
        controller.AddArt(1);
        Destroy(gameObject);
       }
    }
}
