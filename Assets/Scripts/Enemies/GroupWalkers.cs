using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void KillGroup();

public class GroupWalkers : SimpleWalker, IDamageDealable
{
    protected override void Start() {
        base.Start();

        KillGroup dlg = Death;

        for (int i = 0; i < transform.childCount; i++) {
            EnemyParticipant ep = transform.GetChild(i).GetComponent<EnemyParticipant>();
            if(ep == null) {
                continue;
            }

            ep.SetKillGroup(dlg);
        }
    }
}
