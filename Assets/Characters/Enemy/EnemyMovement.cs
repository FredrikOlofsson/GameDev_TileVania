using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [SerializeField] float moveSpeed = 1f;
    Rigidbody2D mrRigidBody;
    SpriteRenderer mrSpriteRenderer;
    BoxCollider2D telescopeCollider;
    void Start()
    {
	   mrRigidBody = GetComponent<Rigidbody2D>();
       mrSpriteRenderer = mrRigidBody.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        mrRigidBody.velocity = new Vector2 (moveSpeed, 0f);
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        print("Exited something!");
        moveSpeed = -moveSpeed;
        transform.localScale = new Vector2(-(Mathf.Sign(mrRigidBody.velocity.x)), transform.localScale.y);
    }
}
