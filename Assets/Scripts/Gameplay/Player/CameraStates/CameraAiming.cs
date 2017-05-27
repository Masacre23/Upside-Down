using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAiming : CameraStates {

    float m_lookAngle = 0.0f;
    float m_tiltAngle = 0.0f;

    bool m_lockingOnTarget = false;
    string m_tagTarget;

    public float m_tiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    public float m_tiltMin = 45f;
    public float m_aimSpeed = 3f;
    public float m_minAngleToTarget = 15.0f;
    public Vector3 m_camPosition = new Vector3(0.0f, 1.3f, 0.0f);

    public override void Start()
    {
        base.Start();
        m_type = States.AIMING;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool returnCam, float timeStep)
    {
        bool ret = false;

        m_variableCam.FollowTarget(timeStep);

        if (m_lockingOnTarget)
            HandleAim(axisHorizontal, axisVertical, timeStep);
        else
            CameraRotation(axisHorizontal, axisVertical, timeStep);

        if (m_variableCam.m_changeCamOnPosition)
        {
            m_variableCam.m_currentState = m_variableCam.m_transit;
            ret = true;
        }

        m_variableCam.m_player.RotateModel(m_variableCam.m_camRay.direction);

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
    }

    void HandleAim(float x, float y, float deltaTime)
    {
        float aimStrength = x * x + y * y;
        //If strength < the minimum input to unlock
        if (aimStrength < m_variableCam.m_targetBreakLock * m_variableCam.m_targetBreakLock)
        {
            GameObject closestTarget = m_variableCam.m_player.FixingOnEnemy(Camera.main.transform, m_minAngleToTarget);

            //If its close to a target
            if (closestTarget)
            {
                Vector3 camToTarget = closestTarget.transform.position - Camera.main.transform.position;
                Quaternion targetRotation = Quaternion.LookRotation(camToTarget.normalized, Camera.main.transform.up);
                m_variableCam.m_cam.rotation = Quaternion.Lerp(m_variableCam.m_cam.rotation, targetRotation, Time.deltaTime);
                m_lookAngle = 0.0f;
                m_tiltAngle = 0.0f;
            }
            //Deal normal rotation
            else
                CameraRotation(x, y, deltaTime);
        }
        //Deal normal rotation
        else
            CameraRotation(x, y, deltaTime);
    }

    void CameraRotation(float x, float y, float deltaTime)
    {
        m_lookAngle += x * m_aimSpeed;
        m_tiltAngle -= y * m_aimSpeed;
        m_tiltAngle = Mathf.Clamp(m_tiltAngle, -m_tiltMin, m_tiltMax);

        Quaternion targetRotation = Quaternion.Euler(m_lookAngle * Vector3.up);
        Quaternion tiltRotation = Quaternion.Euler(m_tiltAngle, m_variableCam.m_pivotEulers.y, m_variableCam.m_pivotEulers.z);

        m_variableCam.m_cam.localRotation = targetRotation * tiltRotation;
    }

    public void SetTargetLock(bool isLocked, string tagToLock)
    {
        m_lockingOnTarget = isLocked;
        m_tagTarget = tagToLock;
    }
}
