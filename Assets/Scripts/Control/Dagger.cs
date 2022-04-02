using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dagger : MonoBehaviour
{
    public PlayerHpIncrease hpHandle;
    private float power = 5f;

    public void SetHpHandleDelegate(PlayerHpIncrease handle) {
        hpHandle = handle;
    }

    public void OnTriggerEnter2D(Collider2D col) {
        BaseEnemy enemy = col.GetComponent<BaseEnemy>();
        if (enemy != null) {
            hpHandle(power);
            enemy.MakeHurt(power);
        }
    }
}
