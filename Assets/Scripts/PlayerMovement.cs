using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    [Header("Variables")]
    public float speed;
    [Header("Components")]
    public Animator bodyAnimator;
    public SpriteRenderer spriteRenderer;
    public Rigidbody2D rigidbody2D;

    private bool canMove = true;
    private Vector2 moveDir = Vector2.zero;


    public bool CanMove
    {
        get { return canMove; }
        set { canMove = value; }
    }
    void Start()
    {
        
    }

    void Update()
    {
        moveDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        if (moveDir != Vector2.zero) { bodyAnimator.SetBool("running", true); }
        else { bodyAnimator.SetBool("running", false); }
        if (moveDir.x < 0) { spriteRenderer.flipX = true; }
        else if (moveDir.x > 0) { spriteRenderer.flipX = false; }
    }

    private void FixedUpdate() //Pohyb
    {
        if (canMove)
        {
            rigidbody2D.velocity = moveDir * speed;
        }
    }
}
