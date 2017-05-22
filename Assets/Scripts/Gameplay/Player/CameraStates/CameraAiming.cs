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
        if (aimStrength < m_variableCam.m_targetBreakLock * m_variableCam.m_targetBreakLock)
        {
            GameObject closestTarget = null;
            float minimumDistance = m_variableCam.m_targetLockDistance * m_variableCam.m_targetLockDistance;
            foreach (GameObject target in m_variableCam.m_player.m_targetsDetectors[m_tagTarget].m_targets)
            {
                GameObject toThrow = ((PlayerThrowing)m_variableCam.m_player.m_throwing).NextObjectThrow();
                if (toThrow && toThrow != target)
                {
                    float distance = Vector3.Cross(m_variableCam.m_camRay.direction, target.transform.position - m_variableCam.m_camRay.origin).sqrMagnitude;
                    if (distance < minimumDistance)
                    {
                        closestTarget = target;
                        minimumDistance = distance;
                    }
                }  
            }

            if (closestTarget)
            {
                m_variableCam.m_cam.LookAt(closestTarget.transform, m_variableCam.m_player.transform.up);
            }
            else
                CameraRotation(x, y, deltaTime);
        }
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
