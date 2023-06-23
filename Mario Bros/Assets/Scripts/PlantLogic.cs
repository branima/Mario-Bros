using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlantLogic : MonoBehaviour
{
    void OnCollisionEnter2D(Collision2D collision) => collision.gameObject.GetComponent<PlayerMovement>().GetHit();
}
