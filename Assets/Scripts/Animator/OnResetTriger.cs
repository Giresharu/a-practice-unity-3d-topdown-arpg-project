using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnResetTriger : MonoBehaviour
{
    private Animator _anim;
    private Animator anim {
        get {
            if (_anim == null)
                _anim = GetComponent<Animator>();
            return _anim;
        }
    }

    public void ResetTriger(string trigerName) {
        anim.ResetTrigger(trigerName);
    }
}
