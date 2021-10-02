using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float speed;
    private Rigidbody2D rb;

    Vector2 movement;

    //public Animator animator;

    private bool facingRight = true;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
    }

    void FixedUpdate() 
    {
        rb.MovePosition(rb.position + movement * speed * Time.fixedDeltaTime);

        //animator.SetFloat("Speed", Mathf.Abs(moveInput));

        //if(facingRight == false && XInput > 0) {
        //    Flip();
        //} else if(facingRight == true && XInput < 0) {
        //    Flip();
        //}
    }

    void Flip ()
    {
        facingRight = !facingRight;
        Vector3 Scaler = transform.localScale;
        Scaler.x *= -1;
        transform.localScale = Scaler;
    }
}
