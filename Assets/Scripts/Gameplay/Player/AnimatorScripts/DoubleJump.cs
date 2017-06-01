using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoubleJump : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("DoubleJump", false);

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

}
