﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFloating : PlayerStates
{
    public float m_maxTimeFloating = 3.0f;
    public float m_risingTime = 1.0f;
    public Vector3 m_floatingPoint;

    float m_timeFloating;
    Vector3 m_startingPosition;

    public override void Start()
    {
        base.Start();
        m_timeFloating = 0.0f;
        m_type = States.FLOATING;
        m_floatingPoint = Vector3.zero;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool pickObjects, bool aimGravity, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;
        HUDManager.ChangeFloatTime(1 - (m_timeFloating / m_maxTimeFloating));

        float perc = m_timeFloating / m_risingTime;
        if (perc > 1.0f)
            perc = 1.0f;
        m_player.m_gravityOnCharacter.Float(m_startingPosition, m_floatingPoint, perc);

        if (m_timeFloating > m_maxTimeFloating)
        {
            m_player.m_currentState = m_player.m_onAir;
            ret = true;
        }
        else
        {
            m_timeFloating += timeStep;
            RaycastHit target;
            bool legalTarget = m_player.m_playerGravity.ViableGravityChange(out target);
            if (changeGravity)
            {
                ret = true;
                if (legalTarget)
                {
                    m_player.m_playerGravity.ChangeGravityTo(target);
                    m_player.m_currentState = m_player.m_onAir;
                    m_player.m_gravityOnCharacter.m_planetGravity = false;
                    m_player.m_gravityOnCharacter.m_changingToAttractor = true;
                }      
                else
                    m_player.m_currentState = m_player.m_onAir;
            }
            else if (aimGravity || jumping)
            {
                ret = true;
                m_player.m_currentState = m_player.m_onAir;
            }
        }   

        return ret;
    }

    public override void OnEnter()
    {
        m_startingPosition = m_player.transform.position;
        m_player.m_camController.SetCameraTransition(CameraStates.States.AIMING);
        //m_player.m_camController.SetAimLockOnTarget(true, "GravityWall");
        m_player.m_rotationFollowPlayer = false;
        m_rigidBody.isKinematic = true;
        m_player.m_reachedGround = false;
        HUDManager.ShowGravityPanel(true);
    }

    public override void OnExit()
    {
        m_rigidBody.isKinematic = false;
        m_player.m_camController.SetCameraTransition(CameraStates.States.BACK);
        //m_player.m_camController.UnsetAimLockOnTarget();
        m_timeFloating = 0.0f;
        HUDManager.ShowGravityPanel(false);
        m_player.m_playerGravity.UnlightObject();
        m_player.m_freezeMovement = true;
    }
}
