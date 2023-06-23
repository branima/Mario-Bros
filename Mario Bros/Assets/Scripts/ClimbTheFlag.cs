using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClimbTheFlag : MonoBehaviour
{

    public float speed;
    Rigidbody2D rb;

    void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        rb.bodyType = RigidbodyType2D.Dynamic;
    }

    void FixedUpdate() => rb.MovePosition(transform.position + Vector3.down * speed * Time.fixedDeltaTime);
}
