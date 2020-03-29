using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeroActorController : ActorController {
    public void OnHoldBowEnter () {
        bow.SetActive (true);
        arrow = GameObject.Instantiate (arrow_zero, arrowNode.transform);
        sword.SetActive (false);
        SetMoveAble (false, true);
        sfxManager.Play ("useItem");
    }
    public void OnReleaseBowEnter () {
        AttackItemManager aim = arrow.GetComponent<AttackItemManager> ();
        aim._dir = new Vector2 (model.transform.forward.x, model.transform.forward.z);
        aim.anim.SetBool ("enable", true);
        sfxManager.Play ("arrow");
    }
    public void OnBowExit () {
        bow.SetActive (false);
        sword.SetActive (true);
    }

}