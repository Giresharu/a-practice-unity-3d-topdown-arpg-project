using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockAttackWall : MonoBehaviour {
    private DamageData dd;
    private AttackItemManager aim;

    private void OnTriggerEnter (Collider col) {
        if (col.tag == "Attack") {
            dd = col.GetComponent<DamageData> ();
            aim = dd.ac.GetComponent<AttackItemManager> ();
            if (aim != null)
                aim.OnBlocked ();
            dd.ac.sfxManager.Play (dd.flickSFX);
            dd.ac.anim.SetTrigger ("flick");
            dd.ac.GetComponentInChildren<FXManager> ().Flick ();
        }
    }
}