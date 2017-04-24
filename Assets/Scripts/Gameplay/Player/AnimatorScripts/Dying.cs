using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dying : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Dead", false);

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

}
