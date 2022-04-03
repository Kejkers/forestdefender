using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour, IDamageDealable
{
    private Rigidbody2D rgbody;
    [SerializeField]private float damage = 1f;
    [SerializeField]private float impulseMultiplier = 1f;
    [SerializeField]protected float destroyDelay = 2f;
    public const float baseImpulse = 50f;

    protected void Start() {
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

    public virtual void ConfirmShot(Transform target) {
        transform.parent = target;
        rgbody.velocity = Vector2.zero;
        StopCoroutine("DestroyMe");
        StartCoroutine("DestroyMe", destroyDelay);
    }

    float IDamageDealable.GetDamage() {
        return damage;
    }

    bool IDamageDealable.IsDealingDamage() {
        return true;
    }

    private IEnumerator DestroyMe(float seconds) {
        yield return new WaitForSeconds(seconds);
        Destroy(this.gameObject);
    }
}
