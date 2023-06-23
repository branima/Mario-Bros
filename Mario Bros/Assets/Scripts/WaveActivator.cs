using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaveActivator : MonoBehaviour
{

    void OnTriggerEnter2D(Collider2D other) => other.GetComponent<IEnemy>().ActivateWave();
}
