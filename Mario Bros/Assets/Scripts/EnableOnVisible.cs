using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnableOnVisible : MonoBehaviour
{

    IEnemy enemyScript;

    void OnBecameVisible()
    {
        enemyScript = GetComponentInParent<IEnemy>();
        enemyScript.ActivateWave();
    }
    void OnBecameInvisible() => enemyScript.DisableWhenInvisible();
}
