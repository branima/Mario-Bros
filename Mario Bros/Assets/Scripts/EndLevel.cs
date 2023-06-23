using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndLevel : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D other) => other.GetComponent<PlayerMovement>().EndLevel();
}
