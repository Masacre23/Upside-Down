using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTransiting : CameraStates {

    public CameraStates m_finalState;
    bool m_transitionStopped = false;

    float m_time;
    float m_addedTime;
    float m_timeBetweenChanges = 0.5f;

    bool m_localTransforms = false;
    bool m_changeWithoutTransition = false;
    Vector3 m_initialPosition;
    Vector3 m_targetPosition;
    Quaternion m_initialRotationPivot;
    Quaternion m_targetRotationPivot;
    Quaternion m_initialRotationCamera;
    Quaternion m_targetRotationCamera;

    Collider m_sphereCam;

    public override void Start()
    {
        base.Start();
        m_type = States.TRANSIT;

        m_sphereCam = m_variableCam.m_cam.GetComponent<Collider>();
    }

    //Main camera update. Returns true if a change in state ocurred (in order to call OnExit() and OnEnter())
    public override bool OnUpdate(float axisHorizontal, float axisVertical, bool returnCam, float timeStep)
    {
        bool ret = false;

        m_variableCam.FollowTarget(timeStep);

        if (m_changeWithoutTransition)
        {
            if (m_localTransforms)
            {
                m_variableCam.m_pivot.localRotation = m_targetRotationPivot;
                m_variableCam.m_cam.localRotation = m_targetRotationCamera;
                m_variableCam.m_cam.localPosition = m_targetPosition;
            }
            else
            {
                m_variableCam.m_pivot.rotation = m_targetRotationPivot;
                m_variableCam.m_cam.rotation = m_targetRotationCamera;
                m_variableCam.m_cam.position = m_targetPosition;
            }

            m_variableCam.m_currentState = m_finalState;
            ret = true;
        }
        else
        {
            m_time += timeStep;
            float perc = m_time / (m_timeBetweenChanges - m_addedTime);

            Quaternion newPivotRotation = Quaternion.Slerp(m_initialRotationPivot, m_targetRotationPivot, perc);
            Quaternion newCameraRotation = Quaternion.Slerp(m_initialRotationCamera, m_targetRotationCamera, perc);
            Vector3 newPosition = Vector3.Lerp(m_initialPosition, m_targetPosition, perc);

            if (m_localTransforms)
            {
                m_variableCam.m_pivot.localRotation = newPivotRotation;
                m_variableCam.m_cam.localRotation = newCameraRotation;
                m_variableCam.m_cam.localPosition = newPosition;
            }
            else
            {
                m_variableCam.m_pivot.rotation = newPivotRotation;
                m_variableCam.m_cam.rotation = newCameraRotation;
                m_variableCam.m_cam.position = newPosition;
            }

            if (perc >= 1.0f)
            {
                m_variableCam.m_currentState = m_finalState;
                ret = true;
            }
        }

        return ret;
    }

    public override void OnEnter()
    {
        m_time = 0.0f;
        m_addedTime = 0.0f;
        m_transitionStopped = false;
        //m_sphereCam.enabled = true;
    }

    public override void OnExit()
    {
        //m_sphereCam.enabled = false;
    }

    public void ResetTime()
    {
        m_addedTime = m_timeBetweenChanges - m_time;
        m_time = 0.0f;
    }

    public void SetTransitionValues(CameraStates finalState, Vector3 targetPosition, Quaternion targetRotationCamera, Quaternion targetRotationPivot, bool localPositions, float changeTotalTime)
    {
        m_localTransforms = localPositions;
        if (m_localTransforms)
        {
            m_initialRotationCamera = m_variableCam.m_cam.localRotation;
            m_initialRotationPivot = m_variableCam.m_pivot.localRotation;
            m_initialPosition = m_variableCam.m_cam.localPosition;
        }
        else
        {
            m_initialRotationCamera = m_variableCam.m_cam.rotation;
            m_initialRotationPivot = m_variableCam.m_pivot.rotation;
            m_initialPosition = m_variableCam.m_cam.position;
        }

        m_finalState = finalState;
        m_targetRotationCamera = targetRotationCamera;
        m_targetRotationPivot = targetRotationPivot;
        m_targetPosition = targetPosition;

        if (changeTotalTime > 0.0f)
        {
            m_changeWithoutTransition = false;
            m_timeBetweenChanges = changeTotalTime;
        } 
        else
        {
            m_changeWithoutTransition = true;
        }        
    }
}
