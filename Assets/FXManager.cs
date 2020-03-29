using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FXManager : MonoBehaviour {

    private ActorManager am;
    public GameObject block;
    public Transform block_node;
    public GameObject hit;
    public Transform hit_node;
    public GameObject flick;
    public Transform flick_node;
    public GameObject dieSmoke;
    public Transform dieSmoke_node;

    private void Awake () {
        am = am??GetComponentInParent<ActorManager> ();
    }
    public void Block () {
        if (block == null)
            return;
        GameObject go = block;
        go.transform.position = block_node.position + Vector3.up * 0.6f + Vector3.forward * -0.5f;
        go.transform.localScale = Vector3.one;
        go.transform.rotation = Quaternion.Euler (45, 0, 0);
        GameObject.Instantiate (go);
    }
    public void Hit () {
        if (hit == null)
            return;
        GameObject go = hit;
        go.transform.position = hit_node.position;
        go.transform.localScale = Vector3.one * 2.5f + Vector3.up * 3.5f;
        go.transform.rotation = Quaternion.Euler (0, 180, 90);
        GameObject.Instantiate (go);
    }
    public void Flick () {
        if (flick == null)
            return;
        GameObject go = flick;
        go.transform.position = flick_node.position + go.transform.forward * -2.5f;
        go.transform.localScale = Vector3.one * 2.5f;
        go.transform.rotation = Quaternion.Euler (45, 0, 0);
        GameObject.Instantiate (go);
    }
    public void DieSmoke () {
        if (dieSmoke == null)
            return;
        GameObject go = dieSmoke;
        go.transform.position = dieSmoke_node.position;
        go.transform.localScale = Vector3.one * 2.5f;
        go.transform.rotation = Quaternion.Euler (45, 0, 0);
        GameObject.Instantiate (go);
    }
}