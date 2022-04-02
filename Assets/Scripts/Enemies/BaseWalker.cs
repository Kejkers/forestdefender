using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseWalker : BaseEnemy
{
    protected new void OnTriggerEnter2D(Collider2D col) {
        base.OnTriggerEnter2D(col);
    }
}
