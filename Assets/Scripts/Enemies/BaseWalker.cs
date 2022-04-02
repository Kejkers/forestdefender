using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWalker : MonoBehaviour
{
    public float hp = 1f;

    protected void OnTriggerEnter2D(Collider2D col) {
        Debug.Log("Shot");
        var proj = col.GetComponent<Projectile>();
        if (proj != null) {
            proj.ConfirmShot(this.transform);
            GetHurt(proj.GetDamage());
        }
    }

    public void GetHurt(float damage) {
        hp -= damage;
        if (hp <= 0f) {
            Death();
        }
    }

    protected void Death() {

    }
}
