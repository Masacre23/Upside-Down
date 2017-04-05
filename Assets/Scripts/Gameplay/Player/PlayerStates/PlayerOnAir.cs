using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerOnAir : PlayerStates
{
    int counter; // Used to set the planet gravity
    public int airTime = 100;

    public override void Start()
    {
        base.Start();
        m_type = States.ONAIR;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool throwing, float timeStep)
    {
        bool ret = false;
        ++counter;
        if (counter > airTime)
        {
            m_player.m_gravityOnCharacter.m_planetGravityActive = true;
            m_player.CheckPlanetStatus();
        }

        if (changeGravity && m_player.m_changeEnabled)
        {
            m_player.m_currentState = m_player.m_floating;
            ret = true;
        }
        else
        {
            m_player.OnAir();
            m_player.UpdateUp();
            if (!m_player.m_freezeMovementOnAir)
                m_player.Move(timeStep);
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
        
    }

    public override void OnExit()
    {
        counter = 0;
        m_player.m_gravityOnCharacter.m_planetGravityActive = false;
    }
}
