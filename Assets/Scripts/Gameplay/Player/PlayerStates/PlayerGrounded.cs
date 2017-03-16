using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerStates {

    public override void Start()
    {
        base.Start();
        m_type = States.GROUNDED;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool throwing, float timeStep)
    {
        bool ret = false;

        if (jumping)
        {
            m_player.m_currentState = m_player.m_onAir;
            m_player.Jump();
            ret = true;
        }
        else
        {
            m_player.UpdateUp();
            m_player.Move(timeStep);
            if (!m_player.CheckGroundStatus())
            {
                m_player.m_currentState = m_player.m_onAir;
                ret = true;
            }            
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_player.m_changeEnabled = true;
        m_player.m_freezeMovementOnAir = false;
    }

    public override void OnExit()
    {

    }
}
