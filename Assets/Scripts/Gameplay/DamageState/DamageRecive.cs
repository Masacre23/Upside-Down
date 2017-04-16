using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRecive : DamageStates {

    public override void Start()
    {
        base.Start();
        m_type = States.RECIVE;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(bool recive, int damage, bool alive)
    {
        bool ret = false;

        if (recive)
        {
            m_charapter.m_health -= damage;
            if(m_charapter.m_health <= 0)
            {
                m_charapter.m_damageState = m_charapter.m_dead;
            }
            else
            {
                m_charapter.m_damageState = m_charapter.m_animation;
            }
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
         
    }

    public override void OnExit()
    {
       
    }
}
