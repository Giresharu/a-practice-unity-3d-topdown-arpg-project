using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BattleManager : MonoBehaviour {

    private ActorManager am;
    private Vector3 hitDir;
    private float atk;
    private float hitDis;

    private DamageData dd;
    private bool immortal;

    private void Awake () {
        am = transform.parent.GetComponent<ActorManager> ();
    }

    private void OnTriggerEnter (Collider col) {
        immortal = am.ac.immortal;
        if (immortal)
            return;
        if (col.tag == "Damage" && am.ac.isPlayer || (col.tag == "Attack" && !am.ac.isPlayer)) {
            dd = col.GetComponent<DamageData> ();
            if (dd.damageType != DamageData.DamageType.ATTACK)
                return;
            hitDir = (transform.position - col.transform.position);
            hitDir = new Vector3 (hitDir.x, transform.position.y, hitDir.z);
            atk = dd.atk;
            hitDis = dd.hitDis;

            Ray ray = new Ray (col.transform.position, hitDir);
            // Debug.DrawRay(col.transform.position,hitDir,Color.red,100);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Sensor"))) {
                //  Debug.Log(hit.collider.name);
                if (hit.collider.transform.tag == "Guard") {
                    am.Blocked (dd.ac, dd.flickSFX);
                    dd.GetComponentInParent<Animator> ().SetTrigger ("flick");
                    return;
                }
            }
            am.DoDamage (dd.ac, atk, hitDir, hitDis, dd.hitSFX);
        }
    }
    private void OnTriggerStay (Collider col) {
        immortal = am.ac.immortal;
        if (immortal)
            return;
        if (col.tag == "Damage" && am.ac.isPlayer || (col.tag == "Attack" && !am.ac.isPlayer)) {
            dd = col.GetComponent<DamageData> ();
            if (dd.damageType == DamageData.DamageType.ATTACK)
                return;
            hitDir = (transform.position - col.transform.position);
            hitDir = new Vector3 (hitDir.x, transform.position.y, hitDir.z);
            atk = dd.atk;
            hitDis = dd.hitDis;

            Ray ray = new Ray (col.transform.position, hitDir);
            // Debug.DrawRay(col.transform.position,hitDir,Color.red,100);
            RaycastHit hit;
            if (Physics.Raycast (ray, out hit, Mathf.Infinity, 1 << LayerMask.NameToLayer ("Sensor"))) {
                //   Debug.Log(hit.collider.name);
                if (hit.collider.transform.tag == "Guard") {
                    am.ac.hitDis = am.ac.strikeDis;
                    am.ac.hitDir = hitDir;
                    am.Blocked (dd.ac, dd.flickSFX);
                    am.ac.Repel ();
                    dd.GetComponentInParent<Animator> ().SetTrigger ("flick");
                    return;
                }
            }
            am.DoDamage (dd.ac, atk, hitDir, hitDis, dd.hitSFX);
        }
    }
}