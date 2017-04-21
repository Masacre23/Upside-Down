using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraAiming : CameraStates {

    float m_lookAngle = 0.0f;
    float m_tiltAngle = 0.0f;

    public bool m_lockingOnTarget = false;
    public string m_tagTarget;

    public override void Start()
    {
        base.Start();
        m_type = States.BACK;
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
                float distance = Vector3.Cross(m_variableCam.m_camRay.direction, target.transform.position - m_variableCam.m_camRay.origin).sqrMagnitude;
                if (distance < minimumDistance)
                {
                    closestTarget = target;
                    minimumDistance = distance;
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
        m_lookAngle += x * m_variableCam.m_aimSpeed;
        m_lookAngle = Mathf.Clamp(m_lookAngle, -m_variableCam.m_lookMinAim, m_variableCam.m_lookMaxAim);
        m_tiltAngle -= y * m_variableCam.m_aimSpeed;
        m_tiltAngle = Mathf.Clamp(m_tiltAngle, -m_variableCam.m_tiltMinAim, m_variableCam.m_tiltMaxAim);

        Quaternion targetRotation = Quaternion.Euler(m_lookAngle * Vector3.up);
        Quaternion tiltRotation = Quaternion.Euler(m_tiltAngle, m_variableCam.m_pivotEulers.y, m_variableCam.m_pivotEulers.z);

        m_variableCam.m_cam.localRotation = targetRotation * tiltRotation;
    }
}
