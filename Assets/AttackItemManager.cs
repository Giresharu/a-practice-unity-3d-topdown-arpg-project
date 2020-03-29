using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackItemManager : MonoBehaviour {
    public Vector2 _dir;
    public GameObject[] _attacks;
    public Vector3 _offset;

    public Animator anim;
    public GameObject rotateNode;

    public ActorController ac;

    private void Awake () {
        ac = ac ?? GetComponent<ActorController> ();
        anim = anim ?? ac.anim;
    }
    private void Update () {
        if (anim.GetBool ("enable")) {
            transform.SetParent (null);
            //要改中间那层的方向
            transform.forward = Vector3.forward;
            rotateNode.transform.forward = Vector3.forward * _dir.normalized.y + Vector3.right * _dir.normalized.x;
            rotateNode.transform.localPosition = rotateNode.transform.right * _offset.x + rotateNode.transform.up * _offset.y + rotateNode.transform.forward * _offset.z;
            ac.targetAxis = _dir.normalized;

            if (Vector3.Distance (transform.position, Camera.main.transform.position) >= 50) {
                Destroy (gameObject);
            }
        }
    }
    public void OnIdleEnter () {
        foreach (GameObject attack in _attacks) {
            attack.SetActive (true);
        }
    }

    public void OnDieEnter () {
        foreach (GameObject attack in _attacks) {
            attack.SetActive (false);
        }
        anim.SetBool ("enable", false);
    }

    public void OnDestroyedEnter () {
        Destroy (gameObject);
    }

    public void OnHitTarget () {
        Destroy (gameObject);
    }

    public void OnBlocked () {
        anim.SetTrigger ("die");
        anim.SetBool ("enable", false);
        ac.targetAxis = Vector3.zero;
        foreach (GameObject attack in _attacks) {
            attack.SetActive (false);
        }
    }
}