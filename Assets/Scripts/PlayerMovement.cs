using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Collider2D mrCollider2D;
    Rigidbody2D mrRigidBody;
    SpriteRenderer mrSpriteRenderer;
    Animator mrAnimator;
    Vector2 moveInput;
    LayerMask layerMaskPlatform;

    [SerializeField] float moveSpeed = 5;

    [SerializeField] float jumpAmountInUnityUnits = 3;
    [SerializeField] float gravityScale = 2;
    [SerializeField] float fallingGravityScale = 3;
    float jumpForce;


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
        Walk();
        flipCharacterSprite();
        if (mrRigidBody.velocity.y >= 0)
        {
            mrRigidBody.gravityScale = gravityScale;
        } else if ((mrRigidBody.velocity.y+0.1f) < 0f)
        {
            mrRigidBody.gravityScale = fallingGravityScale;
        }
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    void OnJump()
    {
        if (mrCollider2D.IsTouchingLayers(layerMaskPlatform))
        {
            mrAnimator.SetTrigger("onJump");
            jumpForce = Mathf.Sqrt(jumpAmountInUnityUnits * -2 * (Physics2D.gravity.y * mrRigidBody.gravityScale));
            mrRigidBody.AddForce(new Vector2(0, jumpForce), ForceMode2D.Impulse);
        } else
        {
            print("I am trying to jump but I cant because I am not on the ground!");
        }        
    }
    private void Walk()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, mrRigidBody.velocity.y);
        mrRigidBody.velocity = playerVelocity;
        walkAnimation();
    }
    private void walkAnimation()
    {
        if (Mathf.Abs(mrRigidBody.velocity.x) > 0)
        {
            mrAnimator.SetBool("isWalking", true);
        } else
        {
            mrAnimator.SetBool("isWalking", false);
        }
    }
    private void flipCharacterSprite()
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