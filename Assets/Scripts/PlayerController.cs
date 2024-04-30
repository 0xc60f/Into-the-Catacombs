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
    private bool _paused;
    int artCount = 1;
    Animator animator;
    public bool _collectedArtForLevel = false;
    Vector2 lookDirection = new Vector2(1, 0);
    public ParticleSystem collectEffect;
    private GameObject _mainCamera;
    public ParticleSystem damageEffect;
    public GameObject _healthCanvas;
    public AudioClip _deathSound;
    public bool _isFrozen = false;
    private AudioSource _audioSource;
    public AudioClip artCollect;
    public AudioClip footstepsLanding;
    public AudioClip hitSound;
    public GameObject _pauseMenu;
    private AudioSource _audioSource1;
    private static readonly int Property = Animator.StringToHash("Look X");

    void Start()
    {

        _mainCamera = GameObject.Find("Main Camera");
        _audioSource1 = _mainCamera.GetComponent<AudioSource>();
        _paused = false;
        _rb = GetComponent<Rigidbody2D>();
        _audioSource = GetComponent<AudioSource>();
        _boxCollider = GetComponent<BoxCollider2D>();
        animator = GetComponent<Animator>();
        _audioSource.PlayOneShot(footstepsLanding);
        _pauseMenu.SetActive(false);
        StartCoroutine(RegenerateHealth());
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            switch (_paused)
            {
                case false:
                    Time.timeScale = 0;
                    //Unpause the background music
                    _audioSource1.Pause();
                    _healthCanvas.SetActive(false);
                    _pauseMenu.SetActive(true);
                    break;
            }
        }

        if (_isFrozen) return;
        _horizontalInput = Input.GetAxis("Horizontal");

        _verticalInput = Input.GetAxis("Vertical");
        Vector2 move = new Vector2(_horizontalInput, _verticalInput);

        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f))
        {
            lookDirection.Set(move.x, move.y);
            lookDirection.Normalize();
        }

        animator.SetFloat(Property, _horizontalInput);

        animator.SetBool("Is Moving", _horizontalInput != 0);

        RaycastHit2D hit = Physics2D.Raycast(_rb.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));
        if (hit.collider == null) return;
        NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>();
        if (character != null)
        {

            character.DisplayDialog();
        }
    }

    private void FixedUpdate()
    {
        if (_isFrozen) return;
        Vector2 pos = _rb.position;
        pos.x += _horizontalInput * speed * Time.deltaTime;
        pos.y += _verticalInput * speed * Time.deltaTime;
        _rb.MovePosition(pos);
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.gameObject.CompareTag("CollisionTiles") && !other.gameObject.CompareTag("Exit")) return;
        if (other.gameObject.CompareTag("Exit"))
        {
            var tileName = other.gameObject.name;
            Debug.Log(tileName);
            if (!_collectedArtForLevel) return;
            other.gameObject.GetComponent<AudioSource>().PlayOneShot(other.gameObject.GetComponent<AudioSource>().clip);
            StartCoroutine(WaitForSewerSound(other.gameObject));
            //Wait until the sound has finished playing

            return;
        }
        //Get the name of the tile you collided with

        if (isInvincible) return;
        _audioSource.PlayOneShot(hitSound);
        if (health > 1)
        {
            health--;
            Debug.Log("Health: " + health);
            //Set the heart child of the canvas to inactive of the health lost
            _healthCanvas.transform.GetChild(health + 1).gameObject.SetActive(false);
            Instantiate(damageEffect, transform.position, Quaternion.identity);
            StartCoroutine(Invincible());
        }
        else
        {
            _healthCanvas.transform.GetChild(1).gameObject.SetActive(false);
            StopCoroutine(nameof(RegenerateHealth));
            _audioSource.PlayOneShot(_deathSound);
            StartCoroutine(WaitForDeathSound());
            return;
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


    public void AddArt(int x)
    {
        artCount += x;
        Instantiate(collectEffect, transform.position, Quaternion.identity);
        _collectedArtForLevel = true;
        //Find the tile object called BlockToFinalBoss
        GameObject blockToFinalBoss = GameObject.Find("BlockToFinalBoss");
        //If the tile object exists, set it to active
        if (blockToFinalBoss != null)
        {
            blockToFinalBoss.SetActive(false);
        }
        var blockToLevelEnd = GameObject.Find("BlockToLevelEnd");
        if (blockToLevelEnd != null)
        {
            blockToLevelEnd.SetActive(false);
        }
        PlayCollectSound();
        
    }

    private void PlayCollectSound()
    {
        _audioSource.PlayOneShot(artCollect);
    }

    //Make an IEnumerator that, every 60 seconds, adds 1 back to the player's health and redraws it on the canvas
    private IEnumerator RegenerateHealth()
    {
        while (true)
        {
            yield return new WaitForSeconds(20);
            if (health >= 6) continue;
            health++;
            _healthCanvas.transform.GetChild(health).gameObject.SetActive(true);
        }
    }
    public IEnumerator WaitForDeathSound()
    {
        _isFrozen = true;
        while (_audioSource.isPlaying)
            yield return null;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        _collectedArtForLevel = false;
        health = 6;
        _isFrozen = false;

    }

    private IEnumerator WaitForSewerSound(GameObject other)
    {
        _isFrozen = true;
        while (other.GetComponent<AudioSource>().isPlaying)
            yield return null;
        //Make the rigidbody of the playe
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        _collectedArtForLevel = false;
        _isFrozen = false;
    }

}
