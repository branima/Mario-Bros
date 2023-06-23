using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PlayerMode
{
    NORMAL, ENLARGED, FLOWER
}

public class PlayerAttributes : MonoBehaviour
{

    PlayerMode playerMode;
    SpriteRenderer playerSpriteRenderer;

    void Start()
    {
        playerMode = PlayerMode.NORMAL;
        playerSpriteRenderer = GetComponentInChildren<SpriteRenderer>();
    }

    public PlayerMode GetPlayerMode() => playerMode;
    public void SetPlayerMode(PlayerMode newMode) => playerMode = newMode;

    public Vector2 GetPlayerOrientation() => playerSpriteRenderer.flipX ? Vector2.left : Vector2.right;
}
