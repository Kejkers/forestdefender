using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleWalker : BaseWalker, IDamageDealable
{
    private GameObject player;
    public float moveSpeed = 1f;
    private float baseMoveSpeed = 0.005f;
    [SerializeField]private bool isDead = false;
    [SerializeField]private float damage = 1f;

    // Start is called before the first frame update
    void Start()
    {
        player = Camera.main.transform.parent.gameObject;
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if (isDead) {
            return;
        }

        Vector3 dir = (player.transform.position - this.transform.position).normalized;
        float speed = moveSpeed * baseMoveSpeed;
        transform.Translate(new Vector3(dir.x * speed, dir.y * speed, 0f));
    }

    public new void MakeHurt(float damage) {
        base.MakeHurt(damage);
        moveSpeed /= 2f;
    }

    protected override void Death() {
        isDead = true;
        StartCoroutine("Dispose");
    }

    float IDamageDealable.GetDamage() {
        return damage;
    }

    bool IDamageDealable.IsDealingDamage() {
        return !isDead;
    }

    private IEnumerator Dispose() {
        yield return new WaitForSeconds(3f);
        Destroy(this.gameObject);
    }
}
