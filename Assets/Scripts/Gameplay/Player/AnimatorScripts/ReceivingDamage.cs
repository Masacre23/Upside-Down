using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReceivingDamage : StateMachineBehaviour
{
    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("Damaged", false);

        base.OnStateEnter(animator, stateInfo, layerIndex);
    }

}
