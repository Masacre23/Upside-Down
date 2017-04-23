using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamageRespawn : DamageStates
{

    private const float m_animtionTime = 0.5f;
    private float m_currentTime;

    public override void Start()
    {
        base.Start();
        m_type = States.RESPAWN;
        m_currentTime = 0.0f;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(DamageData data)
    {
        PlayerRespawn m_respawn = m_charapter.gameObject.GetComponent<PlayerRespawn>();
        m_respawn.ReSpawn(m_charapter.m_checkPoint);
        m_charapter.m_damageState = m_charapter.m_notRecive;
        data.m_respawn = false;

        return true;
    }

    public override void OnEnter()
    {
       
    }

    public override void OnExit()
    {

    }
}

