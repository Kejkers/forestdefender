using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float hp = 1f;

    protected void OnTriggerEnter2D(Collider2D col) {
        var proj = col.GetComponent<Projectile>();
        if (proj != null) {
            proj.ConfirmShot(this.transform);
            MakeHurt(proj.GetDamage());
        }
    }

    public void MakeHurt(float damage) {
        hp -= damage;
        if (hp <= 0f) {
            Death();
        }
    }

    protected void Death() {}
}
