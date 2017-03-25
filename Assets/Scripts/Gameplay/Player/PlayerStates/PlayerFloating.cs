using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloating : PlayerStates
{
    float m_timeFloating;

    public override void Start()
    {
        base.Start();
        m_timeFloating = 0.0f;
        m_type = States.FLOATING;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool throwing, float timeStep)
    {
        bool ret = false;
        HUDManager.ChangeFloatTime(1 - (m_timeFloating / m_player.m_maxTimeFloating));

        if (m_timeFloating > m_player.m_maxTimeFloating)
        {
            m_player.m_currentState = m_player.m_onAir;
            ret = true;
        }
        else
        {
            m_timeFloating += timeStep;
            if (!changeGravity)
            {
                ret = true;
                if (m_player.m_playerGravity.ChangePlayerGravity())
                    m_player.m_currentState = m_player.m_changing;
                else
                    m_player.m_currentState = m_player.m_onAir;
            }
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_rigidBody.isKinematic = true;
        m_player.m_gravitationSphere.SetActive(true);
        m_player.m_changeEnabled = false;
        HUDManager.ShowGravityPanel(true);
    }

    public override void OnExit()
    {
        m_rigidBody.isKinematic = false;
        m_player.m_gravitationSphere.SetActive(false);
        m_timeFloating = 0.0f;
        HUDManager.ShowGravityPanel(false);
    }
}
