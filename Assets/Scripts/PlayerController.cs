using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    public float speed = 5.3f;
    public int health = 6;
Rigidbody2D _rb;
    private float invincibleTime = 1.2f;
    private bool isInvincible = false;
    private BoxCollider2D _boxCollider;
    int artCount = 1;
    Animator animator;
    Vector2 lookDirection = new Vector2(1, 0);
    public ParticleSystem collectEffect;
    public ParticleSystem damageEffect;

    private AudioSource _audioSource;
    public AudioClip artCollect;
    public AudioClip footstepsLanding;
    public AudioClip hitSound;

    void Start()
    {
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        _audioSource.PlayOneShot(footstepsLanding);
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");

        _verticalInput = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(_horizontalInput, _verticalInput);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat("Look X", _horizontalInput);

        if (_horizontalInput != 0)
        {
            animator.SetBool("Is Moving", true);
        }
        else
        {
            animator.SetBool("Is Moving", false);
        }

         RaycastHit2D hit = Physics2D.Raycast(_rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
          if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
                if (character != null)
                {
                   
                    character.DisplayDialog();
                }
            }
    }

    private void FixedUpdate()
    {
        Vector2 pos = _rb.position;
        pos.x += _horizontalInput * speed * Time.deltaTime;
        pos.y += _verticalInput * speed * Time.deltaTime;
        _rb.MovePosition(pos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("CollisionTiles")) return;
        if (isInvincible) return;
        _audioSource.PlayOneShot(hitSound);
        if (health > 1)
        {
            health--;
            Debug.Log("Health: " + health);
            Instantiate(damageEffect, transform.position, Quaternion.identity);
            StartCoroutine(Invincible());
        }
        else
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    private IEnumerator Invincible()
    {
        Physics2D.IgnoreLayerCollision(3, 6, true);
        isInvincible = true;
        yield return new WaitForSeconds(invincibleTime);
        Physics2D.IgnoreLayerCollision(3, 6, false);
        isInvincible = false;
    }

   
    public void addArt(int x){
        artCount +=x;
      
  
            Instantiate(collectEffect, transform.position, Quaternion.identity);
 
    }
    public void PlayCollectSound()
    {
        _audioSource.PlayOneShot(artCollect);

    }

}