using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class VariableCam : MonoBehaviour
{
    public float m_moveSpeed = 1f;
    public float m_turnSpeed = 1.5f;
    public float m_returnSpeed = 1f;

    public bool m_autoReturnCam = false;
    public float m_maxReturnTime = 1f;

    public float m_targetLockDistance = 0.5f;
    public float m_targetBreakLock = 0.15f;

    public bool m_changeCamOnPosition = false;
    public float m_timeBetweenChanges = 0.5f;

    public Ray m_camRay;

    public Player m_player;
    public Transform m_model;
    public Transform m_pivot;
    public Vector3 m_pivotEulers;
    public Transform m_cam;

    public CameraStates m_currentState;
    public CameraStates m_onBack;
    public CameraStates m_aiming;
    public CameraStates m_transit;

    public VariableCameraProtectFromWallClip m_cameraProtection;
    public CameraPlayerDetector m_playerDetector;

    public bool m_followPlayer { set; get; }

    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        m_model = player.transform.FindChild("Model");
        m_pivot = transform.FindChild("Pivot");
        m_pivotEulers = m_pivot.localRotation.eulerAngles;
        m_cam = m_pivot.FindChild("Main Camera");

        m_onBack = GetComponent<CameraOnBack>();
        if (!m_onBack)
            m_onBack = gameObject.AddComponent<CameraOnBack>();

        m_aiming = GetComponent<CameraAiming>();
        if (!m_aiming)
            m_aiming = gameObject.AddComponent<CameraAiming>();

        m_transit = GetComponent<CameraTransiting>();
        if (!m_transit)
            m_transit = gameObject.AddComponent<CameraTransiting>();

        m_currentState = m_onBack;
        m_cam.localPosition = ((CameraOnBack)m_onBack).m_camPosition;
    }

    // Use this for initialization
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        m_player = player.GetComponent<Player>();
        m_cameraProtection = GetComponent<VariableCameraProtectFromWallClip>();
        m_playerDetector = GetComponentInChildren<CameraPlayerDetector>();

        m_camRay = new Ray(m_cam.transform.position, m_cam.transform.forward);
    }

    public void OnUpdate(float axisX, float axisY, bool moveCamBehind, float deltaTime)
    {
        CameraStates previousState = m_currentState;
        if (m_currentState.OnUpdate(axisX, axisY, moveCamBehind, deltaTime))
        {
            previousState.OnExit();
            m_currentState.OnEnter();
        }

        if (m_followPlayer)
        {
            FollowTarget(deltaTime);
            m_followPlayer = !CameraHasReachedPlayer();
        }

        if (!m_followPlayer && !m_playerDetector.m_playerInside)
            m_followPlayer = true;

        m_camRay.origin = m_cam.transform.position;
        m_camRay.direction = m_cam.transform.forward;
    }

    public void FollowTarget(float deltaTime)
    {
        transform.position = Vector3.Lerp(transform.position, m_player.transform.position, deltaTime * m_moveSpeed);
        //transform.position = m_player.transform.position;
    }

    public bool CameraHasReachedPlayer()
    {
        return Vector3.SqrMagnitude(transform.position - m_player.transform.position) < 0.1;
    }

    public void RotateOnTarget(float deltaTime)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, m_player.transform.rotation, deltaTime * m_returnSpeed);
        //transform.rotation = m_player.transform.rotation;
    } 

    public void SetCameraTransition(CameraStates.States finalState, bool alignView = false)
    {
        CameraTransiting transitingCam = (CameraTransiting)m_transit;
        Quaternion rotationPivot;

        transitingCam.ResetTime();
        switch (finalState)
        {
            case CameraStates.States.BACK:
                CameraOnBack onBack = (CameraOnBack)m_onBack;
                rotationPivot = Quaternion.Euler(onBack.m_defaultTiltAngle, m_model.localRotation.eulerAngles.y, 0);
                transitingCam.SetTransitionValues(m_onBack, onBack.m_camPosition, rotationPivot);
                m_changeCamOnPosition = true;
                break;
            case CameraStates.States.AIMING:
                CameraAiming aiming = (CameraAiming)m_aiming;
                if (alignView)
                    rotationPivot = Quaternion.Euler(0, m_pivot.localRotation.eulerAngles.y, 0);
                else
                    rotationPivot = Quaternion.Euler(m_pivot.localRotation.eulerAngles.x, m_pivot.localRotation.eulerAngles.y, 0);
                transitingCam.SetTransitionValues(m_aiming, aiming.m_camPosition, rotationPivot);
                m_changeCamOnPosition = true;
                break;
            default:
                break;
        }
    }

    public void SetAimLockOnTarget(bool isLocked, string tagToLock)
    {
        ((CameraAiming)m_aiming).SetTargetLock(isLocked, tagToLock);
    }

    public void UnsetAimLockOnTarget()
    {
        ((CameraAiming)m_aiming).SetTargetLock(false, null);
    }
}
