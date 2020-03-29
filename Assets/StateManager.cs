using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StateManager : MonoBehaviour {
    public ActorManager am;
    public float HP_max = 3;
    public float HP = 3;

    public bool almostDie;
    public bool healthy;
    public bool minusHP;
    public float _minusHPValue;

    [Header ("1st order states flags")]
    public bool isGround;
    public bool isJump;
    public bool isFall;
    public bool isLand;
    public bool isRoll;
    public bool isAttack;
    public bool isHit;
    public bool isDie;
    public bool isBlocked;
    public bool isDefense;

    [Header ("2st order states flags")]
    public bool isAllowDefense;

    private void Awake () {
        am = am?? GetComponent<ActorManager> ();
    }
    private void Start () {

    }
    private void Update () {
        isGround = am.ac.CheckState ("ground");
        isJump = am.ac.CheckState ("jump");
        isFall = am.ac.CheckState ("fall");
        isLand = am.ac.CheckState ("land");
        isRoll = am.ac.CheckState ("roll");
        isAttack = am.ac.CheckStateTag ("attack");
        isHit = am.ac.CheckState ("hit");
        isDie = am.ac.CheckState ("die");
        isBlocked = am.ac.CheckState ("blocked");
        // isDefense = am.ac.CheckState ("defense", "defense");

        isAllowDefense = isGround || isBlocked;
        isDefense = isAllowDefense && am.ac.CheckState ("defense", "defense");

        if (Input.GetKeyDown (KeyCode.F1)) {
            healthy = true;
            minusHP = true;
        }

        if (almostDie) {
            almostDie = false;
            HP = 0.25f;
        }
        if (healthy) {
            healthy = false;
            AddHP (HP_max);
        }

        if (minusHP) {
            minusHP = false;
            AddHP (-_minusHPValue);
        }
    }
    public void AddHP (float value) {
        HP += value;
        HP = Mathf.Clamp (HP, 0, HP_max);
    }
}