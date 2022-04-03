using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponGrade : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        MainCharacter mc = col.GetComponent<MainCharacter>();
        if (mc == null) {
            return;
        }

        mc.UpgradeArrows();
        Destroy(this);
    }
}
