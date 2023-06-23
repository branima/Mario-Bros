using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum ItemType
{
    SHROOM, FLOWER, COIN
}

public class ItemAttributes : MonoBehaviour
{

    public ItemType itemType;

    Animator blockAnimator;

    void Start() => blockAnimator = GetComponent<Animator>();

    public void ProceedAfterAnimation()
    {
        switch (itemType)
        {
            case ItemType.SHROOM:
                GetComponent<Animator>().enabled = false;
                GetComponent<ShroomMovement>().StartMovement();
                break;
            case ItemType.COIN:
                //Add to coin count
                gameObject.SetActive(false);
                break;
            case ItemType.FLOWER:
                GetComponent<Collider2D>().enabled = true;
                break;
            default:
                break;
        }
    }
}
