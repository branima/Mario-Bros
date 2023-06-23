using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoleDeath : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
            other.GetComponent<PlayerMovement>().Die();
    }
}
