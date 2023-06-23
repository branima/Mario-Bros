using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurtleMovement : MonoBehaviour, IEnemy
{

    Rigidbody2D rb;
    Animator enemyAnimator;
    Collider2D enemyCollider;
    SpriteRenderer turtleSpriteRenderer;

    float direction;
    public float speed;
    public float contactAngleTreshold = 70f;

    bool shellLaunched;

    public List<Transform> waveMembers;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        direction = -1f;
        enemyCollider = GetComponent<Collider2D>();
        turtleSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
        shellLaunched = false;
    }

    void FixedUpdate() => rb.MovePosition(rb.position + Vector2.right * direction * speed * Time.fixedDeltaTime);

    void OnCollisionEnter2D(Collision2D collision)
    {
        string hitLayer = LayerMask.LayerToName(collision.gameObject.layer);
        if (hitLayer == "Obstacle")
        {
            if (collision.GetContact(collision.contactCount - 1).normal.x != 0)
            {
                direction *= -1f;
                turtleSpriteRenderer.flipX = !turtleSpriteRenderer.flipX;
            }
        }
        else if (hitLayer == "Enemy" && shellLaunched)
            collision.gameObject.GetComponent<IEnemy>().DieFromShell();
        else if (hitLayer == "Player")
        {

            if (rb.bodyType == RigidbodyType2D.Dynamic)
            {
                float contactAngle = Mathf.Rad2Deg * Mathf.Acos(collision.GetContact(collision.contactCount - 1).normal.x);
                if (contactAngle >= 90 - contactAngleTreshold && contactAngle <= 90 + contactAngleTreshold)
                {
                    if (!shellLaunched)
                    {
                        Shell();
                        collision.gameObject.GetComponent<PlayerMovement>().BounceOffEnemy();
                    }
                    else
                        Die();
                }
                else
                    collision.gameObject.GetComponent<PlayerMovement>().GetHit();
            }
            else
                LaunchShell(collision.transform.position.x > transform.position.x ? -1f : 1f);
        }
    }

    public void Shell()
    {
        rb.bodyType = RigidbodyType2D.Static;
        enemyAnimator.SetTrigger("stomped");
        this.enabled = false;
    }

    public bool IsLaunched() => shellLaunched;

    public void LaunchShell(float direction)
    {
        this.direction = direction;
        rb.bodyType = RigidbodyType2D.Dynamic;
        this.enabled = true;
        speed *= 5f;
        shellLaunched = true;
    }

    public void Die()
    {
        rb.bodyType = RigidbodyType2D.Static;
        enemyCollider.enabled = false;
        enemyAnimator.SetTrigger("dead");
        this.enabled = false;
    }

    public void DieFromShell() { }

    public void ActivateWave()
    {
        if (waveMembers != null)
            foreach (Transform item in waveMembers)
                item.GetComponent<IEnemy>().ActivateWave();
        this.enabled = true;
    }

    public void DisableWhenInvisible() => gameObject.SetActive(false);


}

