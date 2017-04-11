using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransiting : CameraStates {

    public Vector3 m_initialPosition;
    public Vector3 m_finalPosition;
    public CameraStates m_finalState;
    public bool m_transitionStopped = false;

    float m_time;
    float m_addedTime;
    Quaternion m_targetRotationPivot;
    public Quaternion m_initialRotationPivot;

    Quaternion m_targetRotationCamera;
    public Quaternion m_initialRotationCamera;

    public override void Start()
    {
        base.Start();
        m_type = States.TRANSIT;
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, float timeStep)
    {
        bool ret = false;

        m_variableCam.FollowTarget(timeStep);
        
        m_time += timeStep;
        float perc = m_time / (m_variableCam.m_timeBetweenChanges - m_addedTime);

        Quaternion newPivotRotation = Quaternion.Slerp(m_initialRotationPivot, m_targetRotationPivot, perc);
        m_variableCam.m_pivot.localRotation = newPivotRotation;

        Quaternion newCameraRotation = Quaternion.Slerp(m_initialRotationCamera, m_targetRotationCamera, perc);
        m_variableCam.m_cam.localRotation = newCameraRotation;

        Vector3 newPosition = Vector3.Lerp(m_initialPosition, m_finalPosition, perc);
        m_variableCam.m_cam.localPosition = newPosition;

        if (newPosition.Equals(m_finalPosition))
        {
            m_variableCam.m_currentState = m_finalState;
            ret = true;
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_time = 0.0f;
        m_addedTime = 0.0f;
        m_transitionStopped = false;
        m_targetRotationPivot = Quaternion.Euler(m_variableCam.m_model.localRotation.eulerAngles.y * Vector3.up);
        m_targetRotationCamera = Quaternion.identity;
    }

    public override void OnExit()
    {
    }

    public void ResetTime()
    {
        m_addedTime = m_variableCam.m_timeBetweenChanges - m_time;
        m_time = 0.0f;
    }
}
