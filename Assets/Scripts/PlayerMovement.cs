using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
public class PlayerMovement : MonoBehaviour
{
    Rigidbody2D mrRigidBody;
    SpriteRenderer mrSpriteRenderer;
    Animator mrAnimator;
    Vector2 moveInput;

    [SerializeField]float moveSpeed = 1;
    void Start()
    {
        mrRigidBody = GetComponent<Rigidbody2D>();
        mrSpriteRenderer = GetComponent<SpriteRenderer>();
        mrAnimator = GetComponent<Animator>();
    }
    void Update()
    {
        Walk();
        flipCharacterSprite();
    }
    void OnMove(InputValue value)
    {
        moveInput = value.Get<Vector2>();
    }
    private void Walk()
    {
        Vector2 playerVelocity = new Vector2(moveInput.x * moveSpeed, mrRigidBody.velocity.y);
        mrRigidBody.velocity = playerVelocity;
        print(Mathf.Abs(mrRigidBody.velocity.x));
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
        print(moveInput.x);
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
