using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField]float speed = 5f;
    [SerializeField] float jumpHeight = 5f;
    [SerializeField] float climbSpeed = 5f;
    [SerializeField] Animator fireAnimator;
    [SerializeField] GameObject fire;
    [SerializeField] Transform gun;
    Vector2 moveInput;
    Rigidbody2D myRigidbody;
    Animator animator;
    CapsuleCollider2D capsuleCollider2D;
    BoxCollider2D boxCollider2D;
    SpriteRenderer spriteRenderer;
    
    float startGravity;
    bool isAlive = true;
    void Start()
    {
        myRigidbody = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        capsuleCollider2D = GetComponent<CapsuleCollider2D>();
        startGravity = myRigidbody.gravityScale;
        spriteRenderer = GetComponent<SpriteRenderer>();
        boxCollider2D = GetComponent<BoxCollider2D>();
    }

    void Update()
    {
        if (!isAlive) { return; }
        Run();
        FilpSprite();
        ClimbLadder();
        Die();
    }

    void OnMove(InputValue value)
    {
        if (!isAlive) { return; }
        moveInput = value.Get<Vector2>();
    }

    void OnJump(InputValue value)
    {
        if (!isAlive) { return; }
        if (!boxCollider2D.IsTouchingLayers(LayerMask.GetMask("Ground")))
        {
            
            return;
        }
        if (value.isPressed)
        {
            myRigidbody.velocity += new Vector2(0, jumpHeight);
            //animator.SetBool("isJumping", true);
        }
    }

    void OnFire(InputValue value)
    {
        if (!isAlive) { return; }
        Instantiate(fire, gun.position, transform.rotation);
    }
    

    void Run()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * speed, myRigidbody.velocity.y);
        myRigidbody.velocity  = playerVelocity;
        bool hasMoved = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        animator.SetBool("isRunning", hasMoved);

    }

    void FilpSprite()
    {

        bool hasMoved = Mathf.Abs(myRigidbody.velocity.x) > Mathf.Epsilon;
        if (hasMoved)
        {
            transform.localScale = new Vector2(Mathf.Sign(myRigidbody.velocity.x), 1f);
            //if (myRigidbody.velocity.x > 0)
            //{
            //    spriteRenderer.flipX = false;
            //}
            //else
            //{
            //    spriteRenderer.flipX = true;

            //}
        }
        
        
    }

    void Die()
    {
        if (capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Enermy","Hazard")))
        {
            isAlive = false;
            animator.SetTrigger("Dying");

            //maybe show a canvas that player is dead
            //give the options to reset the scene

            FindObjectOfType<GameSession>().ProcessPlayerDeath();
            //Invoke("ReloadScene", 2f);
        }
    }

    void ReloadScene()
    {
        SceneManager.LoadScene(0);

    }

    void ClimbLadder()
    {
        if (!capsuleCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladder"))) {
            myRigidbody.gravityScale = startGravity;
            animator.SetBool("isClimbing", false);
            return;
        }
        Vector2 climbVelocity = new Vector2(myRigidbody.velocity.x, moveInput.y*climbSpeed);
        myRigidbody.velocity = climbVelocity;
        myRigidbody.gravityScale = 0;

        bool hasClimb = Mathf.Abs(myRigidbody.velocity.y) > Mathf.Epsilon;
        animator.SetBool("isClimbing", hasClimb);
    }
}
