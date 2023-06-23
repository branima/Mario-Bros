using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemy
{
    public void Die();
    public void DieFromShell();
    public void ActivateWave();
    public void DisableWhenInvisible();
}