using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HUD_HP : MonoBehaviour {

    public Canvas cr;
    public static HUD_HP instance;
    public StateManager sm;
    public bool _instantiate;
    public float HP_max;
    private float lastHP_max;
    public float HP;
    private float lastHP;
    private int targetHeartCount;
    private int heartCount;
    public float hurtHeart;
    private int emptyHeartIndex;
    public float offsetX = 10;
    public float offsetY = -10;
    public int heartParRow = 8;
    public float addSpeed;

    public IEnumerator add;

    public GameObject heart_zero;
    public List<Animator> hearts = new List<Animator> ();

    private void Awake () {
        instance = this;
        sm = sm?? GameObject.FindGameObjectWithTag ("Player").GetComponent<StateManager> ();
    }
    private void Start () {
        Instantiate ();
    }

    private void Update () {

        if (_instantiate) {
            Instantiate ();
            _instantiate = false;
        }
        HP = sm.HP;
        if (HP > lastHP) {
            if (add != null)
                StopCoroutine (add);
            add = Add (0.25f);
            StartCoroutine (add);
        }
        HP = sm.HP;
        if (HP < lastHP) {
            if (add != null)
                StopCoroutine (add);
            add = Minus (0.25f);
            StartCoroutine (add);
            //  FreshHP (HP);
        }
        if (HP <= 1 && HP > 0) {
            hearts[0].SetBool ("nearDie", true);
        } else {
            hearts[0].SetBool ("nearDie", false);
        }
        if (HP_max > lastHP_max) {
            LevelUp ();
        }

        lastHP = HP;
        lastHP_max = HP_max;
    }
    private IEnumerator Add (float value) {
        float tempHP = lastHP;
        lastHP = HP;
        while (tempHP < lastHP) {
            tempHP += value;
            if (HP < lastHP) {
                lastHP = HP;
                tempHP = HP;
                FreshHP (Mathf.Clamp (tempHP, 0, lastHP));
                yield break;
            }
            FreshHP (Mathf.Clamp (tempHP, 0, lastHP));
            yield return new WaitForSeconds (addSpeed);
        }
    }

    private IEnumerator Minus (float value) {
        float tempHP = lastHP;
        lastHP = HP;
        while (tempHP > lastHP) {
            tempHP -= value;
            if (HP > lastHP) {
                lastHP = HP;
                tempHP = HP;
                FreshHP (Mathf.Clamp (tempHP, 0, lastHP));
                yield break;
            }
            FreshHP (Mathf.Clamp (tempHP, lastHP, HP_max));
            yield return new WaitForSeconds (addSpeed);
        }
    }

    private void FreshHP (float hp) {
        HeartCompute (hp);
        for (int i = 0; i < emptyHeartIndex - 1; i++) {
            hearts[i].SetTrigger ("full");
        }
        //   if (hurtHeart != 1) {
        MakeHurtHeart (emptyHeartIndex - 1);
        //   }
        for (int i = emptyHeartIndex; i < heartCount; i++) {
            hearts[i].SetTrigger ("empty");
        }
    }

    private void LevelUp () {
        GameObject go = GameObject.Instantiate (heart_zero, transform);
        go.transform.localPosition = (Vector3.right * (heartCount % heartParRow) + Vector3.up * offsetY * Mathf.FloorToInt (heartCount / heartParRow)) * cr.scaleFactor;
        hearts.Add (go.GetComponent<Animator> ());
        hearts[heartCount].SetTrigger ("empty");
        heartCount++;
    }
    private void Instantiate () {
        if (heart_zero == null) {
            Debug.Log ("HUD:HP没有零式体。");
            return;
        }
        if (heartParRow == 0) {
            Debug.Log ("HUD:HP每行不能为0个。");
            return;
        }
        heartCount = 0;
        HP = sm.HP;
        HP_max = sm.HP_max;
        lastHP = HP;
        lastHP_max = HP_max;

        hearts.Clear ();
        foreach (Transform child in transform) {
            GameObject.Destroy (child.gameObject);
        }
        HeartCompute (HP);
        while (heartCount < targetHeartCount) {
            GameObject go = GameObject.Instantiate (heart_zero, transform);
            go.transform.position += Vector3.right * offsetX * (heartCount % heartParRow) + Vector3.up * offsetY * Mathf.FloorToInt (heartCount / heartParRow);
            hearts.Add (go.GetComponent<Animator> ());
            heartCount++;
        }
        FreshHP (HP);
    }

    private void HeartCompute (float tempHP) {
        targetHeartCount = Mathf.CeilToInt (HP_max);
        emptyHeartIndex = targetHeartCount - Mathf.FloorToInt (HP_max - tempHP);
        hurtHeart = 1 - (HP_max - tempHP - Mathf.FloorToInt (HP_max - tempHP));
    }

    private void MakeHurtHeart (int index) {
        if (index < 0 || index >= heartCount) {
            return;
        }
        if (hurtHeart <= 0.25f) {
            hearts[index].SetTrigger ("1of4");
            return;
        }
        if (hurtHeart <= 0.5f) {
            hearts[index].SetTrigger ("2of4");
            return;
        }
        if (hurtHeart <= 0.75f) {
            hearts[index].SetTrigger ("3of4");
            return;
        }
        hearts[index].SetTrigger ("full");
    }
}