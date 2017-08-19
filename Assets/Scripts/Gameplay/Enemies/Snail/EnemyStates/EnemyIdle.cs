using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyIdle : EnemyStates {

	public override void Start ()
    {
		base.Start ();
		m_type = States.IDLE;
        if (m_enemy.m_isSleeping)
        {
            m_enemy.m_animator.SetBool("Sleeping", true);
            m_enemy.m_animator.Play("Sleeping", -1, 0.3f);
            m_enemy.m_animator.speed = 0;
			this.gameObject.transform.GetChild (2).gameObject.SetActive (true);
        }
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

        if (m_enemy.player && !m_enemy.m_isSleeping)
        {
            ret = true;
            m_enemy.m_currentState = m_enemy.m_Following;
        }

        /*if (!m_enemy.m_isSleeping)
        {
            m_enemy.m_animator.SetBool("Sleeping", false);
            m_enemy.m_animator.speed = 1;
        }*/

        return ret;
	}

	public override void OnEnter()
	{
        if (m_enemy.m_animator != null)
        {
            m_enemy.m_animator.SetBool("PlayerDetected", false);
        }
	}

	public override void OnExit()
	{
		this.gameObject.transform.GetChild (2).gameObject.GetComponent<ParticleSystem> ().Stop();
		this.gameObject.transform.GetChild (1).gameObject.SetActive (true);
	}
}
