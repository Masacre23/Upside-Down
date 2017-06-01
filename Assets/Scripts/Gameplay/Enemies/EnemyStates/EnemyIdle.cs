using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyStates {

	public override void Start ()
    {
		base.Start ();
		m_type = States.IDLE;
	}
	
	//Main enemy update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
	public override bool OnUpdate (DamageData data)
    {
		bool ret = false;

        if (data.m_recive)
        {
            ret = true;
            m_enemy.DamageManager(data);
        }

        if (m_enemy.player)
        {
            ret = true;
            m_enemy.m_currentState = m_enemy.m_Following;
        }

		return ret;
	}

	public override void OnEnter()
	{
        if(m_enemy.m_animator != null)
		    m_enemy.m_animator.SetBool("PlayerDetected", false);
	}

	public override void OnExit()
	{
		
	}
}
