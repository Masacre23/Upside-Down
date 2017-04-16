using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageAnimation : DamageStates {

    private const float m_animtionTime = 0.5f;
    private float m_currentTime;

    public override void Start()
    {
        base.Start();
        m_type = States.ANIMATION;
        m_currentTime = 0.0f;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(bool recive, int damage, bool alive)
    {
        bool ret = false;
        m_currentTime += Time.fixedDeltaTime;

        //TODO: Here, we must to play the damage animation. 
        m_charapter.gameObject.transform.Translate((Vector3.back + Vector3.up) * Time.fixedDeltaTime);

        if (m_currentTime >= m_animtionTime)
        {
            m_charapter.m_damageState = m_charapter.m_notRecive;
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_currentTime = 0.0f;
    }

    public override void OnExit()
    {
        m_currentTime = 0.0f;  
    }
}
