using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerGrounded : PlayerStates
{

    public float m_floatingHeight = 1.0f;

    public override void Start()
    {
        base.Start();
        m_type = States.GROUNDED;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;

        if (axisHorizontal == 0.0f && axisVertical == 0.0f)
            m_player.m_playerStopped = true;
        else
            m_player.m_playerStopped = false;

        if (changeGravity && m_player.m_changeButtonReleased)
        {
            m_player.m_currentState = m_player.m_floating;
            m_player.SetFloatingPoint(m_floatingHeight);
            ret = true;
        }
        else if (jumping)
        {
            m_player.m_currentState = m_player.m_onAir;
            m_player.Jump();
            ret = true;
        }
        else if (aimingObject && m_player.m_throwButtonReleased)
        {
            m_player.m_currentState = m_player.m_throwing;
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
        m_player.m_rotationFollowPlayer = true;
        m_player.m_reachedGround = true;
        m_player.m_freezeMovement = false;
    }

    public override void OnExit()
    {

    }
}
