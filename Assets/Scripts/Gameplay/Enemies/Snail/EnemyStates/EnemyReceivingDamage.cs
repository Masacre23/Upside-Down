using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyReceivingDamage : EnemyStates
{
    string m_animationName = "Hit1";

    public override void Start ()
    {
		base.Start();
        m_type = States.RECEIVING;
    }

	public override bool OnUpdate (DamageData data, bool stunned)
    {
		bool ret = false;

        AnimatorStateInfo animatorInfo = m_enemy.m_animator.GetCurrentAnimatorStateInfo(0);
        if (!animatorInfo.IsName(m_animationName))
        {
            ret = true;
            m_enemy.m_currentState = m_enemy.m_Idle;
        }

        if (data.m_recive)
        {
            m_enemy.DamageManager(data);
        }

        return ret;
	}

	public override void OnEnter()
	{
        m_enemy.m_animator.SetBool("Damaged", true);
    }

	public override void OnExit()
	{
        m_enemy.m_animator.SetBool("Damaged", false);
    }
}
