using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorldBorder : MonoBehaviour
{
    void OnTriggerExit2D(Collider2D col) {
        MainCharacter mc = col.GetComponent<MainCharacter>();
        if (mc == null) {
            return;
        }

        mc.GetHurt(150f);
    }
}
