using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ricochete : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col) {
        if(col.GetComponent<Projectile>() != null) {
            float angle = Random.Range(-45f, 45f);
            float frac = Random.Range(-100f, 100f);
            col.transform.Rotate(new Vector3(0f, 0f, angle + frac/100f));

            Rigidbody2D rg2d = col.GetComponent<Rigidbody2D>();
            float vel = Projectile.baseImpulse;
            rg2d.velocity = Vector2.zero;

            float x = Mathf.Sin(col.transform.rotation.z);
            x = (col.transform.rotation.z < 0.7f)? x : -x;
            float y = Mathf.Cos(col.transform.rotation.z);
            y = (col.transform.rotation.z > 0f)? y : -y;

            rg2d.AddForce(new Vector2(x * vel, y * vel));
        }
    }
}
