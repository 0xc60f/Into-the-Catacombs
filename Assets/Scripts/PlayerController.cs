using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private float _horizontalInput;
    private float _verticalInput;
    public float speed = 5.3f;
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        //Moving the player based on horizontal and vertical input. Varies based on time buttons held.
        Vector2 pos = transform.position;
        _horizontalInput = Input.GetAxis("Horizontal");
        _verticalInput = Input.GetAxis("Vertical");
        pos.x += _horizontalInput * Time.deltaTime * speed;
        pos.y += _verticalInput * Time.deltaTime * speed;
        transform.position = pos;
    }
}
