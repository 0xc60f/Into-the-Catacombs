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
    private Rigidbody2D _rb;
    private float invincibleTime = 1.2f;
    private bool isInvincible = false;
    private BoxCollider2D _boxCollider;
    private GridLayout _gridLayout;

    void Start()
    {
        _gridLayout = transform.parent.GetComponentInParent<GridLayout>();
        _rb = GetComponent<Rigidbody2D>();
        _boxCollider = GetComponent<BoxCollider2D>();
    }

    // Update is called once per frame
    void Update()
    {
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
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
        if (health > 0)
        {
            health--;
            Debug.Log("Health: " + health);
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
    private bool PlayerIsOffTile()
    {
        Vector2 playerPosition = transform.position;
        Vector3Int cellPosition = _gridLayout.WorldToCell(playerPosition);
        return cellPosition == new Vector3Int(0, 0, 0) || cellPosition == new Vector3Int(0, 1, 0) || cellPosition == new Vector3Int(1, 0, 0) || cellPosition == new Vector3Int(1, 1, 0);
    }
}
