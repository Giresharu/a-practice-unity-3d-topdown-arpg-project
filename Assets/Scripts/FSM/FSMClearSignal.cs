using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FSMClearSignal : StateMachineBehaviour {

    public string[] clearAtEnter;
    public string[] clearAtExit;

    // OnStateEnter is called when a transition starts and the state machine starts to evaluate this state
    override public void OnStateEnter (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach (string signal in clearAtEnter) {
            animator.ResetTrigger (signal);
        }
        animator.logWarnings = false;
        animator.ResetTrigger ("stateFinished");
        animator.logWarnings = true;
    }

    // OnStateExit is called when a transition ends and the state machine finishes evaluating this state
    override public void OnStateExit (Animator animator, AnimatorStateInfo stateInfo, int layerIndex) {
        foreach (string signal in clearAtExit) {
            animator.ResetTrigger (signal);
        }
    }

    // OnStateMove is called right after Animator.OnAnimatorMove()
    //override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that processes and affects root motion
    //}

    // OnStateIK is called right after Animator.OnAnimatorIK()
    //override public void OnStateIK(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    // Implement code that sets up animation IK (inverse kinematics)
    //}
}