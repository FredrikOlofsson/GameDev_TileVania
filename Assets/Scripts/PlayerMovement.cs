using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public class PlayerMovement : MonoBehaviour
{
    Collider2D mrCollider2D;
    Rigidbody2D mrRigidBody;
    SpriteRenderer mrSpriteRenderer;
    Animator mrAnimator;
    Vector2 moveInput;
    LayerMask layerMaskPlatform;

    [SerializeField] float moveSpeed = 5;

    [Header("Jumping Settings")]
    [SerializeField] float jumpAmountInUnityUnits = 3;
    [SerializeField] float gravityScale = 2;
    [SerializeField] float fallingGravityScale = 3;
    [SerializeField] int maxAmountJumps = 2;
    [SerializeField] int amountOfJumpsLeft;
    float jumpForce;
    bool isClimbing;

    void Start()
    {
        mrCollider2D = GetComponent<Collider2D>();
        mrRigidBody = GetComponent<Rigidbody2D>();
        mrSpriteRenderer = GetComponent<SpriteRenderer>();
        mrAnimator = GetComponent<Animator>();
        layerMaskPlatform = LayerMask.GetMask("Platforms");
    }
    void Update()
    {
        ControllGravtity(); //Double gravity when falling
        Walk(); //Waits for player input
        Climb(); //Only if on a ladder
        ResetJumps(); //Only if standing on a platform
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump()
    {
        if (amountOfJumpsLeft > 0)
        {
            mrAnimator.SetTrigger("onJump");
            jumpForce = Mathf.Sqrt(jumpAmountInUnityUnits * -2 * (Physics2D.gravity.y * mrRigidBody.gravityScale));
            mrRigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
            amountOfJumpsLeft--;
        } else
        {
            print("I am trying to jump but I cant, did I run out of jumps?");
        }
    }
    void ResetJumps()
    {
        if (mrCollider2D.IsTouchingLayers(layerMaskPlatform))
        {
            amountOfJumpsLeft = maxAmountJumps - 1;
        } //Resets the jumps
    }
    private void Walk()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, mrRigidBody.velocity.y);
        mrRigidBody.velocity = playerVelocity;
        FlipCharacterSprite(); //Flips the sprite depending on player input
        WalkAnimation();
    }
    private void WalkAnimation()
    {
        if (Mathf.Abs(mrRigidBody.velocity.x) > 0)
        {
            mrAnimator.SetBool("isWalking", true);
        } else
        {
            mrAnimator.SetBool("isWalking", false);
        }
    }
    private void Climb()
    {
        if (mrCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders")))
        {
            //mrRigidBody.gravityScale = 0;
            if (Mathf.Abs(moveInput.y) > 0)
            {
                mrRigidBody.bodyType = RigidbodyType2D.Kinematic;
                mrRigidBody.velocity = new Vector2(moveInput.x * moveSpeed, 0);
                mrRigidBody.position = new Vector2(mrRigidBody.position.x, mrRigidBody.position.y + moveInput.y * moveSpeed * Time.deltaTime);
            }
        } else
        {
            mrRigidBody.bodyType = RigidbodyType2D.Dynamic;
        }
        ClimbAnimation();
    }
    private void ClimbAnimation()
    {
        if (mrCollider2D.IsTouchingLayers(LayerMask.GetMask("Ladders")) && Mathf.Abs(moveInput.y) > 0)
        {
            mrAnimator.SetBool("isClimbing", true);
        } else
        {
            mrAnimator.SetBool("isClimbing", false);
        }

    }
    private void ControllGravtity()
    {
        if (mrRigidBody.velocity.y >= 0.1f)
        {
            mrRigidBody.gravityScale = gravityScale;
        } else if ((mrRigidBody.velocity.y) < 0.1f)
        {
            mrRigidBody.gravityScale = fallingGravityScale;
        }
    }
    private void FlipCharacterSprite()
    {
        //if moving on the X-axis, flip the character
        if (moveInput.x < 0)
        {
            mrSpriteRenderer.flipX = true;
        } else if (moveInput.x > 0)
        {
            mrSpriteRenderer.flipX = false;
        }
    }
    /* GameDevCode private void flipSprite()
    {
        bool playerHasHorizontalMovement = Mathf.Abs(mrRigidBody.velocity.x) > Mathf.Epsilon;

        transform.localScale = new Vector2(Mathf.Sign(mrRigidBody.velocity.x), 1f);
    }*/
}

//Question Can I have multiple branches from eachother?