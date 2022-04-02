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
        }

        var dd = col.GetComponent<IDamageDealable>();
        if (dd != null && dd.IsDealingDamage()) {
            MakeHurt(dd.GetDamage());
        }
    }

    public void MakeHurt(float damage) {
        Debug.Log(damage);
        Debug.Log(" " + hp);
        hp -= damage;
        Debug.Log(" " + hp);
        if (hp <= 0f) {
            Death();
        }
    }

    protected virtual void Death() {}
}
