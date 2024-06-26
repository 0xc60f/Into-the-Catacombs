using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    [SerializeField] public float vel;

    [SerializeField] BulletHitSparks hitParticles;

    // Start is called before the first frame update
    void Start()
    {
        //Rigidbody2D rb = GetComponent<Rigidbody2D>();
        //rb.AddForce(new Vector2(vel, 0));
    }

    // Update is called once per frame
    void Update()
    {
        this.gameObject.transform.position += transform.up * vel * Time.deltaTime;
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        hitParticles.Flash(c.contacts[0].point);
        if (c.gameObject.tag == "Player"){
            c.gameObject.GetComponent<PlayerController>().ChangeHealth();
        }
        Destroy(gameObject);
    }

    
}
