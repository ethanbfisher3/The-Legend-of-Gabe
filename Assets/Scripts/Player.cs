using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public Rigidbody2D rb;
    public float moveSpeed = 3f;

    void Start()
    {
    }

    void FixedUpdate()
    {
        // make the player move based on the arrow keys
        float moveVertical = 0f;
        float moveHorizontal = 0f;

        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
            moveVertical = moveSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
            moveVertical = -moveSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
            moveHorizontal = -moveSpeed * Time.fixedDeltaTime;
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))

            moveHorizontal = moveSpeed * Time.fixedDeltaTime;

        rb.MovePosition(new Vector2(moveHorizontal + rb.position.x, moveVertical + rb.position.y));

    }
}
