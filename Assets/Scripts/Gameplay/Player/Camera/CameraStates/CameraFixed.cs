using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFixed : CameraStates
{
    Vector3 m_position = Vector3.zero;
    Quaternion m_rotation = Quaternion.identity;

    public override void Start()
    {
        base.Start();
        m_type = States.FIXED;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool returnCam, float timeStep)
    {
        bool ret = false;

        if (m_changeCamState)
        {
            m_variableCam.m_currentState = m_variableCam.m_transit;
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_changeCamState = false;
        m_variableCam.m_cameraProtection.SetProtection(true);

        m_variableCam.m_cam.localRotation = m_rotation;
        m_variableCam.m_cam.localPosition = m_position;
    }

    public override void OnExit()
    {
        m_changeCamState = false;
        m_variableCam.m_cameraProtection.SetProtection(false);
    }

    public void SetTransform(Vector3 position, Quaternion rotation)
    {
        m_position = position;
        m_rotation = rotation;
    }
}
