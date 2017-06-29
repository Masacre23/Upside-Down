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
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool pickObjects, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;

        if (axisHorizontal == 0.0f && axisVertical == 0.0f)
            m_player.m_playerStopped = true;
        else
            m_player.m_playerStopped = false;

        if (jumping)
        {
            m_player.Jump(axisHorizontal, axisVertical);

            m_player.m_currentState = m_player.m_onAir;
            ret = true;
        }
        else if (aimingObject && m_player.m_floatingObjects.HasObjectsToThrow())
        {
            m_player.m_currentState = m_player.m_aimToThrow;
            ret = true;
        }
        else
        {
            m_player.UpdateUp();
            m_player.Move(timeStep);
            m_player.ThrowObjectsThirdPerson(throwing);
            if (pickObjects)
            {
                m_player.PickObjects();
            }
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
        m_player.m_markAimedObject = true;
        m_player.m_rotationFollowPlayer = true;
        m_player.m_reachedGround = true;
        m_player.m_freezeMovement = false;
        
        m_player.m_jumpDirection = Vector3.zero;
        m_player.m_jumpMovement = Vector3.zero;
        m_player.m_rigidBody.velocity = Vector3.zero;
        m_player.m_camController.m_followPlayer = true;

        EffectsManager.Instance.GetEffect(m_player.m_jumpClouds, m_player.m_smoke);
        m_player.m_runClouds.GetComponent<ParticleSystem>().Play();

        if (m_player.m_soundEffects)
            m_player.m_soundEffects.PlaySound("Fall");
    }

    public override void OnExit()
    {
        m_player.m_runClouds.GetComponent<ParticleSystem>().Stop();
        m_player.m_floatingObjects.UnsetTarget();
        m_player.UnmarkTarget();
        m_player.m_rotationFollowPlayer = false;
    }
}
