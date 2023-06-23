using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GoombaMovement : MonoBehaviour, IEnemy
{

    Rigidbody2D rb;
    Animator enemyAnimator;
    Collider2D enemyCollider;

    float direction;
    public float speed;
    public float contactAngleTreshold = 70f;

    public List<Transform> waveMembers;


    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        enemyAnimator = GetComponent<Animator>();
        direction = -1f;
        enemyCollider = GetComponent<Collider2D>();
        this.enabled = false;
    }

    void FixedUpdate() => rb.MovePosition(rb.position + Vector2.right * direction * speed * Time.fixedDeltaTime);

    void OnCollisionEnter2D(Collision2D collision)
    {
        string hitLayer = LayerMask.LayerToName(collision.gameObject.layer);
        if (hitLayer == "Obstacle" || hitLayer == "Enemy")
        {
            if (collision.GetContact(collision.contactCount - 1).normal.x != 0)
                direction *= -1f;
        }
        else if (hitLayer == "Player")
        {
            float contactAngle = Mathf.Rad2Deg * Mathf.Acos(collision.GetContact(collision.contactCount - 1).normal.x);
            if (contactAngle >= 90 - contactAngleTreshold && contactAngle <= 90 + contactAngleTreshold)
            {
                collision.gameObject.GetComponent<PlayerMovement>().BounceOffEnemy();
                Die();
            }
            else
                collision.gameObject.GetComponent<PlayerMovement>().GetHit();
        }
    }

    void _Die()
    {
        UIManager.Instance.AddScore(100);
        rb.bodyType = RigidbodyType2D.Static;
        enemyCollider.enabled = false;
        this.enabled = false;
    }

    public void Die()
    {
        enemyAnimator.SetTrigger("stomped");
        _Die();
    }

    public void DieFromShell()
    {
        enemyAnimator.SetTrigger("shelled");
        _Die();
    }

    public void Disable() => gameObject.SetActive(false);

    public void ActivateWave()
    {
        if (waveMembers != null)
            foreach (Transform item in waveMembers)
                item.GetComponent<IEnemy>().ActivateWave();
        this.enabled = true;
    }

    public void DisableWhenInvisible() => Disable();
}
