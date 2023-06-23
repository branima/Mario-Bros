using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireBallMovement : MonoBehaviour
{

    Animator fireBallAnimator;
    Rigidbody2D rb;

    public float speed;
    Vector2 direction;

    string hitLayer;

    void OnEnable()
    {
        fireBallAnimator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        direction = GameManager.Instance.GetPlayer().GetPlayerOrientation();
    }

    void FixedUpdate() => rb.velocity = new Vector2(direction.x * speed * Time.fixedDeltaTime * 10f, rb.velocity.y);

    void OnCollisionEnter2D(Collision2D collision)
    {
        hitLayer = LayerMask.LayerToName(collision.gameObject.layer);
        if (hitLayer == "Obstacle" || hitLayer == "Enemy")
        {
            if (hitLayer == "Enemy")
                collision.gameObject.GetComponent<IEnemy>().Die();
            fireBallAnimator.SetTrigger("explode");
            rb.bodyType = RigidbodyType2D.Static;
            this.enabled = false;
        }
    }

    public void PoolEnqueue()
    {
        rb.bodyType = RigidbodyType2D.Dynamic;
        this.enabled = true;
        ObjectPooler.Instance.Enqueue("fireball", gameObject);
    }
}
