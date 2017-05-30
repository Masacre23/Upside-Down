using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingEnemyIdle : EnemyStates
{
    // Use this for initialization
    public override void Start()
    {
        base.Start();
        m_type = States.IDLE;
    }

    //Main enemy update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(DamageData data)
    {
        bool ret = false;

        if (data.m_recive)
        {
            ret = true;
            m_enemy.DamageManager(data);
        }

        return ret;
    }

    public override void OnEnter()
    {
     //   m_enemy.m_animator.SetBool("PlayerDetected", false);
    }

    public override void OnExit()
    {

    }
}
