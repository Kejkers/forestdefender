using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public delegate void PlayerHpIncrease(float value);

public class MainCharacter : MonoBehaviour
{
    private GameObject cam;
    private Camera camScript;

    private float hp = 100f;
    private GameObject canvas;
    private RectTransform hpPanel;

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

    private GameObject dagger;
    private bool daggerLock = false;

    void Start(){
        cam = transform.GetChild(1).gameObject;
        camScript = cam.GetComponent<Camera>();

        canvas = transform.GetChild(0).gameObject;
        hpPanel = canvas.transform.GetChild(0).GetComponent<RectTransform>();
        // inevitable
        InvokeRepeating("Curse", 0.5f, 0.5f);

        resultingSpeed = moveSpeed;

        bow = transform.GetChild(2).gameObject;
        bowScript = bow.GetComponent<Bow>();

        sprite = transform.GetChild(3).gameObject;

        dagger = transform.GetChild(4).gameObject;
        PlayerHpIncrease handle = Heal;
        Debug.Log(handle == null);
        dagger.GetComponent<Dagger>().SetHpHandleDelegate(handle);
        dagger.SetActive(false);
    }

    void FixedUpdate()
    {
        Movement();
    }

    void LateUpdate() {
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

    public void AffectSpeed(float mul, float seconds) {
        moveSpeed *= mul;
        StartCoroutine("ResetMoveSpeed", seconds);
    }

    private void Movement() {
        float value = resultingSpeed * baseSpeed;

        if (Input.GetKey(KeyCode.W)) {
            transform.Translate(
                new Vector3(0f, value, 0f)
            );
        }
        if (Input.GetKey(KeyCode.A)) {
            transform.Translate(
                new Vector3(-value, 0f, 0f)
            );
        }
        if (Input.GetKey(KeyCode.S)) {
            transform.Translate(
                new Vector3(0f, -value, 0f)
            );
        }
        if (Input.GetKey(KeyCode.D)) {
            transform.Translate(
                new Vector3(value, 0f, 0f)
            );
        }

        if (Input.GetKey(KeyCode.LeftShift) && !dashLock) {
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

        yield return new WaitForSeconds(duration);

        dagger.SetActive(false);
        bow.SetActive(true);
        daggerLock = false;
    }
}
