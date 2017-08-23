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

    public float m_defaultTimeBetweenChanges = 0.5f;

    public Ray m_camRay;

    public Player m_player;
    public Transform m_model;
    public Transform m_pivot;
    public Vector3 m_pivotEulers;
    public Transform m_cam;

    public CameraStates m_currentState;
    [HideInInspector] public CameraStates m_onBack;
    [HideInInspector] public CameraStates m_transit;
    [HideInInspector] public CameraStates m_onFixedPoint;

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
        m_onFixedPoint = GetComponent<CameraFixed>();
        if (!m_onFixedPoint)
            m_onFixedPoint = gameObject.AddComponent<CameraFixed>();
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

    //This function creates a transition from current camera to onBack camera
    public void SetCameraOnBack(float transitionTime = 0.0f)
    {
        CameraTransiting transitingCam = (CameraTransiting)m_transit;
        transitingCam.ResetTime();

        CameraOnBack onBack = (CameraOnBack)m_onBack;
        Quaternion rotationPivot = Quaternion.Euler(onBack.m_defaultTiltAngle, m_model.localRotation.eulerAngles.y, 0);
        transitingCam.SetTransitionValues(m_onBack, onBack.m_camPosition, Quaternion.identity, rotationPivot, transitionTime);
        m_currentState.EnableCameraChange();
    }

    //This function creates a transition from current camera to Fixed camera
    public void SetCameraOnFixed(Transform camPosition, float transitionTime = 0.0f)
    {
        CameraTransiting transitingCam = (CameraTransiting)m_transit;
        transitingCam.ResetTime();

        CameraFixed fixedCam = (CameraFixed)m_onFixedPoint;
        Quaternion rotationCam = camPosition.rotation;
        
        transitingCam.SetTransitionValues(m_onFixedPoint, camPosition.position, rotationCam, Quaternion.identity, transitionTime);
        fixedCam.SetTransform(camPosition.position, rotationCam);
        m_currentState.EnableCameraChange();
    }

}
