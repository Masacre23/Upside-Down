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
    public override bool OnUpdate(DamageData data)
    {
        bool ret = false;

        if (data.m_recive)
        {
            if (m_charapter is Player)
                ((Player)m_charapter).ChangeCurrntStateToOnAir();
            m_charapter.m_health -= data.m_damage;
            if(m_charapter.m_health <= 0)
            {
                m_charapter.m_damageState = m_charapter.m_dead;
            }
            else
            {
                if (data.m_respawn)
                {
                    m_charapter.m_damageState = m_charapter.m_respawn;
                }else
                {
                    m_charapter.m_damageState = m_charapter.m_animation;
                }
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
