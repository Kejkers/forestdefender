using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    private Rigidbody2D rgbody;
    [SerializeField]private float damage = 1f;
    [SerializeField]private float impulseMultiplier = 1f;
    private readonly float baseImpulse = 50f;

    void Start() {
        rgbody = GetComponent<Rigidbody2D>();
        StartCoroutine("DestroyMe", 4f);
        Launch();
    }

    public void Launch() {
        Vector3 point = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        Vector3 direction = (point - transform.position).normalized;
        float x = direction.x * baseImpulse * impulseMultiplier;
        float y = direction.y * baseImpulse * impulseMultiplier;
        rgbody.AddForce(new Vector2(x, y), ForceMode2D.Impulse);
    }

    public void ConfirmShot(Transform target) {
        transform.parent = target;
        rgbody.velocity = Vector2.zero;
        StopCoroutine("DestroyMe");
        StartCoroutine("DestroyMe", 2f);
    }

    public float GetDamage() {
        return damage;
    }

    private IEnumerator DestroyMe(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
