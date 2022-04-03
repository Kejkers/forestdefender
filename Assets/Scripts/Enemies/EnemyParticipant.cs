using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyParticipant : BaseWalker
{
    protected GroupWalkers group;
    private KillGroup killback;

    protected new void Start() {
        group = transform.parent.GetComponent<GroupWalkers>();
    }

    public override  void MakeHurt(float damage) {
        base.MakeHurt(damage);
    }

    public void SetKillGroup(KillGroup killgroup) {
        killback = killgroup;
    }

    protected override void Death() {
        killback();
        base.Death();
    }
}
