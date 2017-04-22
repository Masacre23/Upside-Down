using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageDead : DamageStates {

    public override void Start()
    {
        base.Start();
        m_type = States.DEAD;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(DamageData data)
    {
        if(m_charapter is Player)
        {
            Scenes.LoadScene(Scenes.GameOver);
        }
        if(data.m_alive)
        {
            m_charapter.m_damageState = m_charapter.m_notRecive;
        }
        return data.m_alive;
    }

    public override void OnEnter()
    {
        m_charapter.m_damage.m_alive = false;
		m_charapter.m_animator.SetBool ("Dead", true);
    }

    public override void OnExit()
    {
       
    }
}
