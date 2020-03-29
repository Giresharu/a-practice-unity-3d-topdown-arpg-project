using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterSignal : MonoBehaviour {
    public enum State {
        IDLE,
        // MOVE,
        ATTACK
    }
    public State state;

    private ActorController ac;

    private void Awake () {
        ac = GetComponent<ActorController> ();
    }
    IEnumerator Start () {
        while (true) {
            if (state == State.ATTACK) {
                ac.newAttack = true;
                yield return new WaitForSeconds (0.05f);

            }
            if (state == State.IDLE) {
                yield return new WaitForSeconds (0.05f);
            }
        }
    }
}