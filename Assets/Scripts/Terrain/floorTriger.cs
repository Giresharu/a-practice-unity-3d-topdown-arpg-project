using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class floorTriger : MonoBehaviour {
    public List<Collider> jumpBorders;
    private ActorController ac;
    private bool haveSet = false;

    private void Awake () {
        foreach (Transform tran in transform) {
            if (tran != transform)
                jumpBorders.Add (tran.GetComponent<Collider> ());
        }

        ac = GameObject.FindGameObjectWithTag ("Player").GetComponent<ActorController> ();
        foreach (Collider col in jumpBorders) {
            col.enabled = false;
        }
    }

    private void OnTriggerEnter (Collider other) {
        if (ac.CheckState ("jump") && !ac.CheckNextState ("land"))
            return;
        if (other.tag == "Player") {
            foreach (Collider col in jumpBorders) {
                col.enabled = true;
            }
            haveSet = true;
            print ("set");
        }
    }

    private void OnTriggerStay (Collider other) {
        if (ac.CheckState ("jump") && !ac.CheckNextState ("land"))
            return;
        if (other.tag == "Player") {
            if (haveSet)
                return;
            print ("haven't set");
            foreach (Collider col in jumpBorders) {
                col.enabled = true;
            }
            haveSet = true;
            print ("set");
        }
    }

    private void OnTriggerExit (Collider other) {
        if (other.tag == "Player") {
            foreach (Collider col in jumpBorders) {
                if (col.tag == "JumpBorder") {
                    col.enabled = false;
                }
            }
            haveSet = false;
        }
    }
}