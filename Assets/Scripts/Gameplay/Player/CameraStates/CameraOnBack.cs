using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraOnBack : CameraStates
{
    float m_returnTime = 0.0f;
    float m_lookAngle = 0.0f;
    float m_tiltAngle = 0.0f;

    public float m_defaultTiltAngle = 45f;
    public float m_tiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    public float m_tiltMin = 45f;
    public Vector3 m_camPosition = new Vector3(0.0f, 2.0f, -5.0f);

    public override void Start()
    {
        base.Start();
        m_type = States.BACK;
        m_tiltAngle = m_defaultTiltAngle;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool returnCam, float timeStep)
    {
        bool ret = false;

        m_variableCam.FollowTarget(timeStep);
        CameraRotation(axisHorizontal, axisVertical, timeStep);

        if (m_variableCam.m_changeCamOnPosition)
        {
            m_variableCam.m_currentState = m_variableCam.m_transit;
            ret = true;
        }
        else if (returnCam)
        {
            m_variableCam.SetCameraTransition(m_type);
            m_variableCam.m_currentState = m_variableCam.m_transit;
            ret = true;
        }
        else if (m_variableCam.m_autoReturnCam)
        {
            if (m_variableCam.m_player.m_playerStopped && axisHorizontal == 0 && axisVertical == 0)
            {
                m_returnTime += timeStep;
                if (m_returnTime > m_variableCam.m_maxReturnTime)
                {
                    m_variableCam.SetCameraTransition(m_type);
                    m_variableCam.m_currentState = m_variableCam.m_transit;
                    ret = true;
                }
            }
            else
                m_returnTime = 0.0f;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_returnTime = m_variableCam.m_maxReturnTime;
        m_variableCam.m_changeCamOnPosition = false;
        m_variableCam.m_cameraProtection.SetProtection(true);
    }

    public override void OnExit()
    {
        m_returnTime = 0.0f;
        m_variableCam.m_changeCamOnPosition = false;
        m_variableCam.m_cameraProtection.SetProtection(false);
        m_lookAngle = m_variableCam.m_model.localRotation.eulerAngles.y;
        m_tiltAngle = m_defaultTiltAngle;
    }

    void CameraRotation(float x, float y, float deltaTime)
    {
        m_lookAngle += x * m_variableCam.m_turnSpeed;
        m_tiltAngle -= y * m_variableCam.m_turnSpeed;
        m_tiltAngle = Mathf.Clamp(m_tiltAngle, -m_tiltMin, m_tiltMax);

        Quaternion targetRotation = Quaternion.Euler(m_lookAngle * Vector3.up);
        Quaternion tiltRotation = Quaternion.Euler(m_tiltAngle, m_variableCam.m_pivotEulers.y, m_variableCam.m_pivotEulers.z);

        m_variableCam.m_pivot.localRotation = targetRotation * tiltRotation;
    }

    void MoveCamBehind(float deltaTime)
    {
        m_lookAngle = m_variableCam.m_model.localRotation.eulerAngles.y;
        m_tiltAngle = 0.0f;

        Quaternion targetRotation = Quaternion.Euler(m_lookAngle * Vector3.up);
        Quaternion tiltRotation = Quaternion.Euler(m_tiltAngle, m_variableCam.m_pivotEulers.y, m_variableCam.m_pivotEulers.z);

        m_variableCam.m_pivot.localRotation = Quaternion.Lerp(m_variableCam.m_pivot.localRotation, targetRotation * tiltRotation, deltaTime * m_variableCam.m_returnSpeed);
    }
}
