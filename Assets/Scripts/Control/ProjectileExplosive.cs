using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileExplosive : Projectile {
    private GameObject explosion;

    protected new void Start() {
        base.Start();
        explosion = transform.GetChild(0).gameObject;
    }

    public override void ConfirmShot(Transform target) {
        base.ConfirmShot(target);
        explosion.SetActive(true);
        Destroy(GetComponent<SpriteRenderer>());
        StartCoroutine("DestroyMe", destroyDelay);
    }
}
