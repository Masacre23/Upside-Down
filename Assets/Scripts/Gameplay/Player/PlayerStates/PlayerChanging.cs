using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerChanging : PlayerStates
{
    Quaternion m_initialRotation;
    Quaternion m_finalRotation;

    bool m_changedGravity = false;
    RaycastHit m_targetPosition;
    float m_totalDistanceToTarget;
    float m_currentDistanceToTarget;

    float jumpInputHorizontal;
    float jumpInputVertical;

    public override void Start()
    {
        base.Start();
        m_type = States.CHANGING;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool pickObjects, bool aimGravity, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;

        m_player.OnAir();
        m_player.MoveOnAir(timeStep);

        m_currentDistanceToTarget = (m_targetPosition.point - transform.position).magnitude;
        float perc = 1 - (m_currentDistanceToTarget / m_totalDistanceToTarget);
        //m_currentDistanceToTarget -= 0.1f;

        m_player.transform.rotation = Quaternion.Lerp(m_initialRotation, m_finalRotation, perc);

        if (!m_changedGravity && Vector3.Dot(m_rigidBody.velocity, transform.up) < 0)
        {
            //m_player.m_playerGravity.ChangeGravityTo(m_targetPosition);
            m_player.m_gravityOnCharacter.ChangeGravityToPoint(m_targetPosition, transform.position);
            m_player.m_gravityOnCharacter.ChangeToAttractor();
            m_player.m_rigidBody.velocity = Vector3.zero;
            m_changedGravity = true;
        }

        if (m_changedGravity)
        {

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
        m_player.m_negatePlayerInput = true;
        m_changedGravity = false;

        m_initialRotation = transform.rotation;

        m_finalRotation = Quaternion.FromToRotation(transform.up, m_targetPosition.normal) * transform.rotation;

        m_currentDistanceToTarget = (m_targetPosition.point - transform.position).magnitude;
    }

    public override void OnExit()
    {
        m_player.m_negatePlayerInput = false;
    }

    public void SetChanging(RaycastHit hitLocation)
    {
        m_targetPosition = hitLocation;
        m_totalDistanceToTarget = (hitLocation.point - transform.position).magnitude;
    }
}
