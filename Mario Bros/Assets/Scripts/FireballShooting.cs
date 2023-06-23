using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FireballShooting : MonoBehaviour
{

    public float gapBetweenShots;
    float lastShotTime;

    Transform newFireball;

    void OnEnable() => lastShotTime = float.MinValue;

    void Update()
    {
        if (Input.GetAxisRaw("Fire1") > 0 && Time.time - lastShotTime >= gapBetweenShots)
        {
            lastShotTime = Time.time;
            newFireball = ObjectPooler.Instance.SpawnFromPool("fireball").transform;
            newFireball.position = transform.position + Vector3.up * 0.16f;
        }
    }
}
