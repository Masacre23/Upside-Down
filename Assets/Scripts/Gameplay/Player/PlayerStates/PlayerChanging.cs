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
    Vector3 m_directionChange;

    float m_timeJumping = 0.0f;

    public override void Start()
    {
        base.Start();
        m_type = States.CHANGING;
    }

    //Main player update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool jumping, bool pickObjects, bool changeGravity, bool aimingObject, bool throwing, float timeStep)
    {
        bool ret = false;

        m_player.OnAir();

        m_currentDistanceToTarget = (m_targetPosition.point - transform.position).magnitude;
        float perc = 1 - (m_currentDistanceToTarget / m_totalDistanceToTarget);

        m_player.transform.rotation = Quaternion.Lerp(m_initialRotation, m_finalRotation, perc);

        if (m_changedGravity)
        {
            if (m_player.CheckGroundStatus())
            {
                m_player.m_currentState = m_player.m_grounded;
                ret = true;
            }
        }
        else
        {
            m_player.MoveOnAir(timeStep);
            m_timeJumping += timeStep;
            if (Vector3.Dot(m_rigidBody.velocity, transform.up) < 0 || m_timeJumping > 3.0f)
            {
                UpdateLanding();
                //m_player.m_gravityOnCharacter.ChangeGravityToPoint(m_targetPosition, transform.position);
                //m_player.m_gravityOnCharacter.ChangeToAttractor();
                m_player.m_rigidBody.velocity = Vector3.zero;
                m_player.m_jumpDirection = Vector3.zero;
                m_changedGravity = true;
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

        m_player.JumpInDirection(m_player.m_modelTransform.forward, 1.0f);

        //m_player.m_gravityOnCharacter.m_changingToAttractor = true;
        m_timeJumping = 0.0f;
    }

    public override void OnExit()
    {
        m_player.m_negatePlayerInput = false;
    }

    public void SetChanging(RaycastHit hitLocation)
    {
        m_targetPosition = hitLocation;
        m_directionChange = hitLocation.point - transform.position;
        m_totalDistanceToTarget = m_directionChange.magnitude;
    }

    private void UpdateLanding()
    {
        int layerMask = 1 << LayerMask.NameToLayer("Terrain");
        RaycastHit target;
        if (Physics.Raycast(m_player.transform.position, m_directionChange.normalized, out target, m_player.m_gravityRange, layerMask))
        {
            if (target.transform.gameObject == m_targetPosition.transform.gameObject)
            {
                m_targetPosition = target;
                m_totalDistanceToTarget = (m_targetPosition.point - transform.position).magnitude;
                m_initialRotation = transform.rotation;
                m_finalRotation = Quaternion.FromToRotation(transform.up, m_targetPosition.normal) * transform.rotation;
            }
        }
    }
}
