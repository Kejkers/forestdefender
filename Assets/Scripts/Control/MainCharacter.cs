using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public delegate void PlayerHpIncrease(float value);
public delegate void DaggerStatusControl(bool control);

public class MainCharacter : MonoBehaviour
{
    private GameObject cam;
    private Camera camScript;

    private float hp = 100f;
    private GameObject canvas;
    private RectTransform hpPanel;
    private GameObject upgradeEffect;

    private const int MINUS_SIGN = -1;
    private const int PLUS_SIGN = 1;

    // local speed multipliers (like dash) and global (like debuffs)
    private float resultingSpeed = 0f;
    // speed multiplier open for buffs and debuffs from outside
    public float moveSpeed = 1f;
    private float baseSpeed = 0.05f;

    private bool dashLock = false;
    private bool dashDone = false;

    private GameObject bow;
    private Bow bowScript;

    private GameObject sprite;
    private Animator anim;

    private GameObject dagger;
    private bool daggerLock = false;

    void Start(){
        cam = transform.GetChild(1).gameObject;
        camScript = cam.GetComponent<Camera>();

        canvas = transform.GetChild(0).gameObject;
        hpPanel = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        upgradeEffect = canvas.transform.GetChild(1).gameObject;
        // inevitable
        InvokeRepeating("Curse", 0.5f, 0.5f);

        resultingSpeed = moveSpeed;

        bow = transform.GetChild(2).gameObject;
        bowScript = bow.GetComponent<Bow>();

        sprite = transform.GetChild(3).gameObject;
        anim = sprite.GetComponent<Animator>();

        dagger = transform.GetChild(4).gameObject;
        PlayerHpIncrease hphandle = Heal;
        dagger.GetComponent<Dagger>().SetHpHandleDelegate(hphandle);

        dagger.SetActive(false);

    }

    void FixedUpdate()
    {
        if (hp <= 0) {
            return;
        }

        Movement();
    }

    void LateUpdate() {
        if (hp <= 0) {
            return;
        }

        Aim();
        Dagger();
    }

    public void GetHurt(float value) {
        // Dash gives i-frames
        if (!dashLock) {
            ChangeHp(value, MINUS_SIGN);
        }
    }

    public void Heal(float value) {
        ChangeHp(value, PLUS_SIGN);
    }

    public void UpgradeArrows() {
        bowScript.UpgradeArrows();
        StartCoroutine("ShowWeaponUpgrade");
    }

    public void AffectSpeed(float mul, float seconds) {
        moveSpeed *= mul;
        StartCoroutine("ResetMoveSpeed", seconds);
    }

    private void Movement() {
        bool move = false;
        float value = resultingSpeed * baseSpeed;

        if (Input.GetKey(KeyCode.W)) {
            move = true;
            transform.Translate(
                new Vector3(0f, value, 0f)
            );
        }
        if (Input.GetKey(KeyCode.A)) {
            move = true;
            transform.Translate(
                new Vector3(-value, 0f, 0f)
            );
        }
        if (Input.GetKey(KeyCode.S)) {
            move = true;
            transform.Translate(
                new Vector3(0f, -value, 0f)
            );
        }
        if (Input.GetKey(KeyCode.D)) {
            move = true;
            transform.Translate(
                new Vector3(value, 0f, 0f)
            );
        }

        anim.SetBool("walk", move);

        if (Input.GetKey(KeyCode.LeftShift) && !dashLock) {
            anim.SetBool("walk", true);
            StartCoroutine("Dash");
        } else if (!(Input.GetKey(KeyCode.LeftShift) && !dashLock && dashDone)) {
            dashLock = false;
        }
    }

    private IEnumerator Dash() {
        dashDone = false;
        dashLock = true;

        resultingSpeed = moveSpeed * 2.5f;
        yield return new WaitForSeconds(0.25f);
        resultingSpeed = moveSpeed;

        if (!Input.GetKey(KeyCode.LeftShift)) {
            dashLock = false;
        }
        dashDone = true;
    }

    private IEnumerator ResetMoveSpeed(float seconds) {
        yield return new WaitForSeconds(seconds);
        moveSpeed = 1f;
    }

    private void ChangeHp(float value, int sign) {
        if (hp <= 0f || hp + sign * value <= 0f) {
            hp = 0f;
            return;
        } else if (hp >= 145f) {
            hp = 150f;
            return;
        }

        hp += sign * value;
        hpPanel.localScale = new Vector3(
            hpPanel.localScale.x + sign * (value / 100f),
            hpPanel.localScale.y,
            hpPanel.localScale.z
        );
    }

    private void Curse() {
        ChangeHp(1.5f, MINUS_SIGN);
    }

    private void Aim() {
        Vector3 point = camScript.ScreenToWorldPoint(Input.mousePosition);
        point.z = 0f;

        Vector3 direction = (point - transform.position).normalized;
        float sign = (transform.position.x < point.x)? 1f : -1f;

        sprite.transform.localScale = new Vector3(
            sign * transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        dagger.transform.localScale = new Vector3(
            sign * transform.localScale.x,
            transform.localScale.y,
            transform.localScale.z
        );

        float rotz = Mathf.Atan2(direction.y, direction.x);
        bow.transform.localRotation = Quaternion.Euler(0f, 0f, rotz * Mathf.Rad2Deg);
        bow.transform.localPosition = new Vector3(
            direction.x * 0.15f,
            direction.y * 0.139f,
            -0.01f
        );

        if (Input.GetMouseButtonDown(0)) {
            bowScript.Fire();
        }
    }

    private void Dagger() {
        if (!daggerLock && Input.GetMouseButtonDown(1)) {
            StartCoroutine("DaggerStab", 0.45f);
        }
    }

    private IEnumerator DaggerStab(float duration) {
        daggerLock = true;
        bow.SetActive(false);
        dagger.SetActive(true);
        resultingSpeed = moveSpeed / 10f;
        anim.SetBool("dagger", true);

        yield return new WaitForSeconds(duration);

        anim.SetBool("dagger", false);
        resultingSpeed = moveSpeed;
        dagger.SetActive(false);
        bow.SetActive(true);
        daggerLock = false;
    }

    public void OnTriggerStay2D(Collider2D col) {
        // disable damage from our own projectiles
        if (col.GetComponent<Projectile>() != null) {
            return;
        }

        IDamageDealable dd = col.GetComponent<IDamageDealable>();
        if (dd != null && dd.IsDealingDamage()) {
            this.GetHurt(dd.GetDamage());
        }
    }

    private IEnumerator ShowWeaponUpgrade() {
        upgradeEffect.SetActive(true);
        yield return new WaitForSeconds(2.5f);
        upgradeEffect.SetActive(false);
    }
}
