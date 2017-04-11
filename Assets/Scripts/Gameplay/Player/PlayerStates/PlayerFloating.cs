using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloating : PlayerStates
{
    float m_timeFloating;
    public Vector3 m_floatingPoint;
    Vector3 m_startingPosition;

    public override void Start()
    {
        base.Start();
        m_timeFloating = 0.0f;
        m_type = States.FLOATING;
        m_floatingPoint = Vector3.zero;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool throwing, float timeStep)
    {
        bool ret = false;
        HUDManager.ChangeFloatTime(1 - (m_timeFloating / m_player.m_maxTimeFloating));

        float perc = m_timeFloating / m_player.m_maxTimeFloating;
        perc = 2 * perc - perc * perc * perc;
        m_player.transform.position = Vector3.Slerp(m_startingPosition, m_floatingPoint, perc);

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
                {
                    m_player.m_currentState = m_player.m_onAir;
                    m_player.m_gravityOnCharacter.m_planetGravity = false;
                }      
                else
                    m_player.m_currentState = m_player.m_onAir;
            }
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_startingPosition = m_player.transform.position;
        m_player.m_mainCam.SetCameraTransition(CameraStates.States.AIMING);
        m_player.m_rotationFollowPlayer = false;
        m_rigidBody.isKinematic = true;
        m_player.m_gravitationSphere.SetActive(true);
        m_player.m_reachedGround = false;
        m_player.m_changeButtonReleased = false;
        HUDManager.ShowGravityPanel(true);
    }

    public override void OnExit()
    {
        m_rigidBody.isKinematic = false;
        m_player.m_mainCam.SetCameraTransition(CameraStates.States.BACK);
        m_player.m_gravitationSphere.SetActive(false);
        m_timeFloating = 0.0f;
        HUDManager.ShowGravityPanel(false);
    }
}
