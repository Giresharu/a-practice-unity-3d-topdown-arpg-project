using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorManager : MonoBehaviour {

    public ActorController ac;
    public BattleManager bm;
    public StateManager sm;
    public FXManager fm;

    private IEnumerator combatPause;

    private void Awake () {
        ac = ac?? GetComponent<ActorController> ();
        bm = bm?? GetComponentInChildren<BattleManager> ();
        sm = sm?? GetComponent<StateManager> ();
        fm = fm?? GetComponentInChildren<FXManager> ();
    }
    /// <summary>
    /// DoDamage
    /// </summary>
    /// <param name="hitDir"></param>
    /// <param name="hitSpeedScale"></param>
    public bool DoDamage (ActorController damegeFrom, float atk, Vector3 hitDir, float hitDis, string hitSFX = "") {
        ac.hitDir = hitDir;
        ac.hitDis = hitDis;

        sm.AddHP (-atk);
        ac.immortal = true;
        damegeFrom.sfxManager.Play (hitSFX);
        damegeFrom.SendMessage ("OnHitTarget");
        Hit ();
        if (sm.HP == 0) {
            Die ();
        }

        return true;
    }

    public void Blocked (ActorController damegeFrom, string flickSFX = "") {
        fm.Block ();
        damegeFrom.sfxManager.Play (flickSFX);
        ac.IssueTrigger ("blocked");
        damegeFrom.SendMessage ("OnBlocked");
    }

    public void Hit () {
        ac.IssueTrigger ("hit");
        if (tag == "Enemy") {
            fm.Hit ();
        }
    }

    public void Die () {

        ac.IssueTrigger ("die");
        if (tag == "Enemy") {
            if (combatPause != null)
                StopCoroutine (combatPause);
            combatPause = CombatPause (0.3f);
            StartCoroutine (combatPause);
            StartCoroutine (DelayDie (0.4f));
        }
    }

    public IEnumerator CombatPause (float _time) {
        Time.timeScale = 0;
        yield return new WaitForSecondsRealtime (_time);
        Time.timeScale = 1;
    }

    public IEnumerator DelayDie (float _time) {
        yield return new WaitForSecondsRealtime (_time);
        fm.DieSmoke ();
        ac.sfxManager.Play ("die");
        DestroyImmediate (this.gameObject);
    }
}