using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class VariableCam : MonoBehaviour
{
    public float m_moveSpeed = 1f;
    public float m_rotateSpeed = 1f;
    public float m_turnSpeed = 1.5f;
    public bool m_autoReturnCam = false;
    public float m_maxReturnTime = 1f;

    public float m_tiltMaxBack = 75f;                       // The maximum value of the x axis rotation of the pivot.
    public float m_tiltMinBack = 45f;
    public float m_tiltMaxTop = 75f;                       // The maximum value of the x axis rotation of the pivot.
    public float m_tiltMinTop = 45f;
    public float m_tiltMaxAim = 75f;                       // The maximum value of the x axis rotation of the pivot.
    public float m_tiltMinAim = 45f;
    public float m_aimSpeed = 1.5f;
    public float m_lookMaxAim = 80f;                       // The maximum value of the x axis rotation of the pivot.
    public float m_lookMinAim = -80f;

    public float m_targetLockDistance = 0.5f;
    public float m_targetBreakLock = 0.15f;

    [SerializeField] Vector3 m_backCamPosition;
    [SerializeField] Vector3 m_aimingCamPosition;
    [SerializeField] Vector3 m_topCamPosition;
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
    public CameraStates m_onTop;
    public CameraStates m_aiming;
    public CameraStates m_transit;

    public VariableCameraProtectFromWallClip m_cameraProtection;

    void Awake()
    {
        GameObject player = GameObject.Find("Player");
        m_model = player.transform.FindChild("Model");
        m_pivot = transform.FindChild("Pivot");
        m_pivotEulers = m_pivot.localRotation.eulerAngles;
        m_cam = m_pivot.FindChild("Main Camera");

        m_onBack = gameObject.AddComponent<CameraOnBack>();
        //m_onTop = gameObject.AddComponent<CameraOnTop>();
        m_aiming = gameObject.AddComponent<CameraAiming>();
        m_transit = gameObject.AddComponent<CameraTransiting>();
    }

    // Use this for initialization
    void Start()
    {
        GameObject player = GameObject.Find("Player");
        m_player = player.GetComponent<Player>();
        m_cameraProtection = GetComponent<VariableCameraProtectFromWallClip>();

        m_currentState = m_onBack;
        m_cam.localPosition = m_backCamPosition;

        //m_currentState = m_onTop;
        //m_cam.localPosition = m_topCamPosition;

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

        m_camRay.origin = m_cam.transform.position;
        m_camRay.direction = m_cam.transform.forward;
    }

    public void FollowTarget(float deltaTime)
    {
        transform.position = Vector3.Lerp(transform.position, m_player.transform.position, deltaTime * m_moveSpeed);
    }

    public void RotateOnTarget(float deltaTime)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, m_player.transform.rotation, deltaTime * m_rotateSpeed);
    } 

    public void SetCameraTransition(CameraStates.States finalState)
    {
        CameraTransiting transitingCam = (CameraTransiting)m_transit;

        transitingCam.m_initialRotationCamera = m_cam.localRotation;
        transitingCam.m_initialRotationPivot = m_pivot.localRotation;
        transitingCam.m_initialPosition = m_cam.localPosition;
        transitingCam.ResetTime();

        switch (finalState)
        {
            case CameraStates.States.BACK:
                transitingCam.m_finalPosition = m_backCamPosition;
                transitingCam.m_finalState = m_onBack;
                m_changeCamOnPosition = true;
                break;
            case CameraStates.States.TOP:
                transitingCam.m_finalPosition = m_topCamPosition;
                transitingCam.m_finalState = m_onTop;
                m_changeCamOnPosition = true;
                break;
            case CameraStates.States.AIMING:
                transitingCam.m_finalPosition = m_aimingCamPosition;
                transitingCam.m_finalState = m_aiming;
                m_changeCamOnPosition = true;
                break;
            default:
                break;
        }
    }

    public void SetAimLockOnTarget(bool isLocked, string tagToLock)
    {
        CameraAiming aiming = (CameraAiming)m_aiming;
        aiming.m_lockingOnTarget = isLocked;
        aiming.m_tagTarget = tagToLock;
    }

    public void UnsetAimLockOnTarget()
    {
        CameraAiming aiming = (CameraAiming)m_aiming;
        aiming.m_lockingOnTarget = false;
        aiming.m_tagTarget = null;
    }
}
