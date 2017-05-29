using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAir : PlayerStates
{
    private bool m_doubleJump = false;

    public override void Start()
    {
        base.Start();
        m_type = States.ONAIR;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool pickObjects, bool aimGravity, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;

        if (axisHorizontal == 0.0f && axisVertical == 0.0f)
            m_player.m_playerStopped = true;
        else
            m_player.m_playerStopped = false;

        if (jumping && !m_doubleJump)
        {
            m_player.Jump();
            m_doubleJump = true;
            m_player.m_doubleJumping = true;
            m_player.m_oxigen.LostOxigen(20);
        }
        if (m_player.m_reachedGround && (aimGravity || changeGravity))
        {
            m_player.m_currentState = m_player.m_floating;
            m_player.SetFloatingPoint(0.0f);
            ret = true;
        }
        else
        {
            m_player.OnAir();
            m_player.UpdateUp();
            m_player.MoveOnAir(timeStep);
            if (m_player.CheckGroundStatus())
            {
                m_player.m_currentState = m_player.m_grounded;
                ret = true;
            }
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_doubleJump = false;   
    }

    public override void OnExit()
    {
    }
}
