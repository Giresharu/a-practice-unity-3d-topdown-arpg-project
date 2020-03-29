using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RootMotionController : MonoBehaviour {
    private Animator _anim;
    private Animator anim {
        get {
            if (_anim == null)
                _anim = GetComponent<Animator> ();
            return _anim;
        }
    }
    // private void OnAnimatorMove () {
    //     // if (anim.GetCurrentAnimatorStateInfo (0).IsName ("jump"))
    //     //     anim.applyRootMotion = true;
    //     // else
    //     //     anim.applyRootMotion = false;
    // }
}