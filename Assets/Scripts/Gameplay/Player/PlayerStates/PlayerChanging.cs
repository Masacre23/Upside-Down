using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChanging : PlayerStates
{
    float m_timeChanging;
    Quaternion m_initialRotation;
    Quaternion m_finalRotation;

    public override void Start()
    {
        base.Start();
        m_timeChanging = 0.0f;
        m_type = States.CHANGING;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;

        m_timeChanging += timeStep;
        float perc = m_timeChanging / m_player.m_maxTimeChanging;
        transform.rotation = Quaternion.Lerp(m_initialRotation, m_finalRotation, perc);

        if (m_timeChanging > m_player.m_maxTimeChanging)
        {
            m_player.m_currentState = m_player.m_onAir;
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_rigidBody.isKinematic = true;
        m_player.m_gravitationSphere.SetActive(true);
        m_initialRotation = transform.rotation;
        m_finalRotation = Quaternion.FromToRotation(transform.up, m_player.m_gravityOnCharacter.m_gravity) * transform.rotation;
    }

    public override void OnExit()
    {
        m_rigidBody.isKinematic = false;
        m_player.m_gravitationSphere.SetActive(false);
        m_timeChanging = 0.0f;
    }
}
