using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{

    Transform player;
    float offsetX;

    bool follow;

    void Start() => player = GameManager.Instance.GetPlayer().transform;

    void Update()
    {
        if (!follow && player.position.x >= transform.position.x)
        {
            if (player == null)
                player = GameManager.Instance.GetPlayer().transform;
            offsetX = transform.position.x - player.position.x;
            follow = true;
        }
    }

    void LateUpdate()
    {
        if (follow)
            transform.position = new Vector3(player.position.x + offsetX, transform.position.y, -1);
    }

    public void Unfollow() => follow = false;
}
