using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpBorder : MonoBehaviour {

    private ActorController ac;
    private Collider col;

    public bool debug_Off;

    private void Awake () {
        try {
            ac = GameObject.FindGameObjectWithTag("Player").GetComponent<ActorController>();
        } catch {
            ac = null;
        }

        col = GetComponent<Collider> ();
    }

    /*     private void Update () {
            if (ac.CheckState ("jump") || ac.CheckNextState ("jump") && !ac.CheckNextState ("land"))
                col.isTrigger = true;
            else
                col.isTrigger = false;
        } */
    private void OnCollisionStay (Collision collision) {
        if (ac == null) {
            try {
                ac = GameObject.FindGameObjectWithTag("Player").GetComponent<ActorController>();
            } catch {
                Debug.Log("没有玩家");
                return;
            }
        }
        if (debug_Off)
            return;
        if (collision.gameObject.tag == "Player" && PlayerInput.instance.AxisMag > 0.99f) {
            print ("与空气墙的夹角是：" + Vector3.Angle (transform.forward, ac.model.transform.forward));
            if (Vector3.Angle (transform.forward, ac.model.transform.forward) < 10 && ac.isGround) {
                ac.Jump ();
                col.enabled = false;
            }

        }
    }
}