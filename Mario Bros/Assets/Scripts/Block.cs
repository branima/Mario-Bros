using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BlockType
{
    BASIC, QUESTION, SOLID, COIN
}

public class Block : MonoBehaviour
{

    public BlockType blockType;

    PlayerAttributes player;
    Animator blockAnimator;
    SpriteRenderer blockSpriteRenderer;
    ParticleSystem smashPS;

    void Start()
    {
        player = GameManager.Instance.GetPlayer();
        blockAnimator = GetComponent<Animator>();
        blockSpriteRenderer = transform.GetChild(0).GetComponent<SpriteRenderer>();
        if (transform.childCount > 1)
            smashPS = transform.GetChild(1).GetComponent<ParticleSystem>();
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.transform.position.y > transform.position.y || other.GetContact(other.contactCount - 1).normal.y < 0.95f)
            return;

        switch (blockType)
        {
            case BlockType.BASIC:
                if (player.GetPlayerMode() != PlayerMode.NORMAL)
                {
                    blockSpriteRenderer.enabled = false;
                    smashPS.transform.parent = null;
                    smashPS.Play();
                    gameObject.SetActive(false);
                }
                else
                    blockAnimator.SetTrigger("wobble");
                break;
            case BlockType.QUESTION:
                blockType = BlockType.SOLID;
                Transform item;
                if (player.GetPlayerMode() == PlayerMode.NORMAL)
                    item = ObjectPooler.Instance.SpawnFromPool("shroom").transform;
                else
                    item = ObjectPooler.Instance.SpawnFromPool("flower").transform;
                item.parent = transform;
                item.localPosition = Vector3.zero;
                blockAnimator.SetTrigger("powerOut");
                break;
            case BlockType.COIN:
                blockType = BlockType.SOLID;
                Transform coin = ObjectPooler.Instance.SpawnFromPool("coin").transform;
                coin.parent = transform;
                coin.localPosition = Vector3.zero;
                blockAnimator.SetTrigger("powerOut");
                UIManager.Instance.AddCoin();
                break;
            default:
                break;
        }
    }

    public void DisableAnimator() => blockAnimator.enabled = false;
}
