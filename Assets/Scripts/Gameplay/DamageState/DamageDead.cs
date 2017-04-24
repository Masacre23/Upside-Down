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
        if(m_character is Player)
        {
            Scenes.LoadScene(Scenes.GameOver);
        }
        if(data.m_alive)
        {
            m_character.m_damageState = m_character.m_notRecive;
        }
        return data.m_alive;
    }

    public override void OnEnter()
    {
        m_character.m_damage.m_alive = false;
        m_character.m_animator.SetBool("Dead", true);
    }

    public override void OnExit()
    {
       
    }
}
