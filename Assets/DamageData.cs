using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageData : MonoBehaviour {
    public enum DamageType {
        ATTACK,
        BODY,
        CANTBLOCK
    }
    public DamageType damageType;

    public ActorController ac;
    public float atk = 1;
    public float hitDis = -1;
    public string hitSFX;
    public string flickSFX = "flick";

    private void Awake () {
        if (transform.parent.parent.TryGetComponent<ActorController> (out ac)) {

        } else {
            transform.parent.parent.parent.TryGetComponent<ActorController> (out ac);
        }
    }

}