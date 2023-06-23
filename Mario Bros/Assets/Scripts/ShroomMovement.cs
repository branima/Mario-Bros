using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShroomMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Collider2D shroomCollider;
    SpriteRenderer spriteRenderer;

    float direction;
    public float speed;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        shroomCollider = GetComponent<Collider2D>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        direction = 1f;
    }

    void FixedUpdate()
    {
        if (shroomCollider.enabled)
            rb.MovePosition(rb.position + Vector2.right * direction * speed * Time.fixedDeltaTime);
    }
    
    public void StartMovement()
    {
        transform.parent = null;
        rb.bodyType = RigidbodyType2D.Dynamic;
        shroomCollider.enabled = true;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string hitLayer = LayerMask.LayerToName(collision.gameObject.layer);
        if (hitLayer == "Obstacle")
        {
            if (collision.GetContact(collision.contactCount - 1).normal.x != 0)
            {
                direction *= -1f;
                spriteRenderer.flipX = !spriteRenderer.flipX;
            }
        }
        else if (hitLayer == "Player")
        {
            collision.gameObject.GetComponent<PlayerMovement>().ShroomCollected();
            gameObject.SetActive(false);
        }
    }
}
