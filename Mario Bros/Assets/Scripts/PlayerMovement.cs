using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    Rigidbody2D rb;
    Animator playerAnimator;
    SpriteRenderer spriteRenderer;
    PlayerAttributes playerAttributes;
    FireballShooting fireballShooting;
    CameraFollow cameraFollow;
    ClimbTheFlag climbTheFlag;

    public CapsuleCollider2D mainCollider;

    public float speed = 20f;
    [Range(0.0f, 1.0f)]
    public float smoothTime = 0f;

    public float jumpSpeed = 50f;
    public float maxJumpHeight;
    float startHeight;
    [Range(0.0f, 0.15f)]
    public float jumpSmoothTime = 0f;

    [Range(1.0f, 2.0f)]
    public float boostFactor = 1.75f;
    float boost;

    float inputX, inputY, oldInputY;
    float directionX, directionY;

    float velocityRef, boostRef, directionYRef = 0f;

    [SerializeField]
    bool isGrounded;
    bool hasHitJumpLimit;

    bool hasJumped;

    public float invincibleDuration;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        spriteRenderer = GetComponentInChildren<SpriteRenderer>();
        playerAttributes = GetComponent<PlayerAttributes>();
        fireballShooting = GetComponent<FireballShooting>();
        cameraFollow = Camera.main.GetComponent<CameraFollow>();
        climbTheFlag = GetComponent<ClimbTheFlag>();
        directionX = directionY = boost = 0;
        inputX = inputY = oldInputY = 0f;
        isGrounded = true;
        hasHitJumpLimit = false;
        startHeight = transform.position.y;
        hasJumped = false;
    }

    void Update()
    {
        inputX = Input.GetAxis("Horizontal");
        oldInputY = inputY;
        inputY = Input.GetAxisRaw("Vertical");

        if ((oldInputY == 0 && inputY > 0) && hasJumped)
            inputY = 0;
        else if (inputY > 0 && !hasJumped)
            hasJumped = true;
        else if (oldInputY == inputY & isGrounded)
            hasJumped = false;

        boost = Input.GetAxisRaw("Sprint") > 0 ? Mathf.SmoothDamp(boost, boostFactor, ref boostRef, smoothTime / 2f) : Mathf.SmoothDamp(boost, 1, ref boostRef, smoothTime / 2f);

        if (rb.velocity.x * inputX < 0f)
            playerAnimator.SetTrigger("sliding");
        else
        {
            if (directionY != 0)
                playerAnimator.SetTrigger("jumping");
            else if (Mathf.Abs(rb.velocity.x) > 0.05f)
                playerAnimator.SetTrigger("running");
            else
                playerAnimator.SetTrigger("idle");
        }

        if (inputX > 0)
            spriteRenderer.flipX = false;
        else if (inputX < 0)
        {
            spriteRenderer.flipX = true;
            cameraFollow.Unfollow();
        }

        if (transform.position.y - startHeight > maxJumpHeight * 0.75f)
            hasHitJumpLimit = true;

        if (inputY > 0 && !hasHitJumpLimit)
            directionY = 1f;

        else if (!isGrounded)
            directionY = Mathf.SmoothDamp(directionY, -1f, ref directionYRef, jumpSmoothTime);
    }

    void FixedUpdate()
    {
        rb.velocity = Vector2.right * Mathf.SmoothDamp(rb.velocity.x, inputX * speed * boost * Time.fixedDeltaTime, ref velocityRef, smoothTime) + Vector2.up * directionY * jumpSpeed * Time.fixedDeltaTime;
        isGrounded = false;
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        string hitLayer = LayerMask.LayerToName(collision.gameObject.layer);
        if (hitLayer == "Ground" || hitLayer == "Obstacle")
        {
            if (collision.GetContact(collision.contactCount - 1).normal.y < -0.9f)
            {
                directionY = 0;
                hasHitJumpLimit = true;
            }
        }
    }

    void OnTriggerStay2D(Collider2D other)
    {
        string hitLayer = LayerMask.LayerToName(other.gameObject.layer);
        if ((hitLayer == "Ground" || hitLayer == "Obstacle"))
        {
            isGrounded = true;
            startHeight = rb.position.y;
            hasHitJumpLimit = false;
            directionY = 0f;
            inputY = 0f;
        }
    }


    public void ShroomCollected()
    {
        playerAnimator.SetTrigger("grow");
        this.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        UIManager.Instance.AddScore(1000);
    }

    public void FlowerCollected()
    {
        playerAnimator.SetTrigger("flowerPickUp");
        fireballShooting.enabled = true;
        UIManager.Instance.AddScore(1000);
    }

    public void BounceOffEnemy() => directionY = 1f;

    public void GetHit()
    {
        mainCollider.enabled = false;
        this.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;

        if (playerAttributes.GetPlayerMode() == PlayerMode.ENLARGED || playerAttributes.GetPlayerMode() == PlayerMode.FLOWER)
            playerAnimator.SetTrigger("shrink");
        else
            playerAnimator.SetTrigger("death");
    }

    public void Die()
    {
        mainCollider.enabled = false;
        this.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        playerAnimator.SetTrigger("death");
    }

    public void Grow()
    {
        mainCollider.offset = 2 * mainCollider.offset;
        mainCollider.size = new Vector2(0.16f, 0.32f);
        playerAttributes.SetPlayerMode(PlayerMode.ENLARGED);
        this.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    public void Shrink()
    {
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), true);
        Invoke("BecomeVulnerable", invincibleDuration);
        playerAnimator.SetBool("invincible", true);
        mainCollider.offset = 0.5f * mainCollider.offset;
        mainCollider.size = new Vector2(0.1f, 0.16f);
        mainCollider.enabled = true;
        playerAttributes.SetPlayerMode(PlayerMode.NORMAL);
        this.enabled = true;
        rb.bodyType = RigidbodyType2D.Dynamic;
        fireballShooting.enabled = false;
    }

    void BecomeVulnerable()
    {
        playerAnimator.SetBool("invincible", false);
        Physics2D.IgnoreLayerCollision(LayerMask.NameToLayer("Player"), LayerMask.NameToLayer("Enemy"), false);
    }

    public void FlagCapture()
    {
        rb.bodyType = RigidbodyType2D.Static;
        playerAnimator.SetTrigger("climb");
        this.enabled = false;
        climbTheFlag.enabled = true;
        UIManager.Instance.AddScore(1000);
    }

    public void EndLevel()
    {
        rb.bodyType = RigidbodyType2D.Static;
        climbTheFlag.enabled = false;
        playerAnimator.SetTrigger("end");
    }

    public void Win() => GameManager.Instance.LevelComplete();

    public void Defeat() => GameManager.Instance.Retry();
}
