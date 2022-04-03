using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    public float hp = 1f;
    protected Animator anim;

    protected virtual void Start() {
        anim = GetComponent<Animator>();
    }

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

    public virtual void MakeHurt(float damage) {
        hp -= damage;
        if (hp <= 0f) {
            Death();
        }
    }

    protected virtual void Death() {
        if(anim != null) {
            anim.SetBool("dead", true);
        }
    }
}
