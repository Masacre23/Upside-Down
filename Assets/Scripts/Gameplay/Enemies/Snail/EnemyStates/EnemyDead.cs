using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyDead : EnemyStates
{
    string m_animationName = "Death";

    public override void Start ()
    {
		base.Start();
        m_type = States.DEAD;
    }

	public override bool OnUpdate (DamageData data)
    {
		bool ret = false;

        //AnimatorStateInfo animatorInfo = m_enemy.m_animator.GetCurrentAnimatorStateInfo(0);
        //if (animatorInfo.IsName(m_animationName) && animatorInfo.normalizedTime >= 1.0f)
        //{
        //    OnExit(data);
        //}

        return ret;
	}

	public override void OnEnter()
	{
        if (m_enemy.m_type == Enemy.Types.SNAIL)
            m_enemy.m_animator.SetBool("Dead", true);
        else
            this.gameObject.SetActive(false);
    }

	public override void OnExit()
	{
        if (m_enemy.m_type == Enemy.Types.SNAIL)
            m_enemy.m_animator.SetBool("Dead", false);
    }
}
