using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoleLogic : MonoBehaviour
{

    Animator poleAnimator;

    void Start() => poleAnimator = GetComponent<Animator>();


    void OnTriggerEnter2D(Collider2D other)
    {
        poleAnimator.enabled = true;
        other.GetComponent<PlayerMovement>().FlagCapture();
    }
}
