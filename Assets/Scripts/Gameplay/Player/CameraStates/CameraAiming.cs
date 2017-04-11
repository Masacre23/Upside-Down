using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAiming : CameraStates {

    float m_lookAngle = 0.0f;
    float m_tiltAngle = 0.0f;

    public override void Start()
    {
        base.Start();
        m_type = States.BACK;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, float timeStep)
    {
        bool ret = false;

        m_variableCam.m_playerScript.m_playerGravity.DrawRay();

        m_variableCam.FollowTarget(timeStep);
        CameraRotation(axisHorizontal, axisVertical, timeStep);

        if (m_variableCam.m_changeCamOnPosition)
        {
            m_variableCam.m_currentState = m_variableCam.m_transit;
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_lookAngle = 0.0f;
        m_tiltAngle = 0.0f;
        m_variableCam.m_changeCamOnPosition = false;
    }

    public override void OnExit()
    {
        m_variableCam.m_changeCamOnPosition = false;
        m_variableCam.m_playerScript.m_playerGravity.DisableRay();
    }

    void CameraRotation(float x, float y, float deltaTime)
    {
        m_lookAngle += x * m_variableCam.m_turnSpeed;
        m_tiltAngle -= y * m_variableCam.m_turnSpeed;
        m_tiltAngle = Mathf.Clamp(m_tiltAngle, -m_variableCam.m_tiltMin, m_variableCam.m_tiltMax);

        Quaternion targetRotation = Quaternion.Euler(m_lookAngle * Vector3.up);
        Quaternion tiltRotation = Quaternion.Euler(m_tiltAngle, m_variableCam.m_pivotEulers.y, m_variableCam.m_pivotEulers.z);

        m_variableCam.m_cam.localRotation = targetRotation * tiltRotation;
    }
}
