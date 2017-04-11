using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class VariableCam : MonoBehaviour
{
    public float m_moveSpeed = 1f;
    public float m_rotateSpeed = 1f;
    [Range(0f, 10f)] public float m_turnSpeed = 1.5f;
    public float m_maxReturnTime = 1f;
    public float m_tiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    public float m_tiltMin = 45f;

    Vector3 m_initialCamPosition;
    [SerializeField] Vector3 m_aimingCamPosition;
    public bool m_changeCamOnPosition = false;
    public float m_timeBetweenChanges = 0.5f;

    GameObject m_player;
    public Player m_playerScript;
    public Transform m_model;
    public Transform m_pivot;
    public Vector3 m_pivotEulers;
    public Transform m_cam;

    public CameraStates m_currentState;
    public CameraStates m_onBack;
    public CameraStates m_aiming;
    public CameraStates m_transit;

    public VariableCameraProtectFromWallClip m_cameraProtection;

    void Awake()
    {
        m_player = GameObject.Find("Player");
        m_model = m_player.transform.FindChild("Model");
        m_pivot = transform.FindChild("Pivot");
        m_pivotEulers = m_pivot.localRotation.eulerAngles;
        m_cam = m_pivot.FindChild("Main Camera");

        m_onBack = gameObject.AddComponent<CameraOnBack>();
        m_aiming = gameObject.AddComponent<CameraAiming>();
        m_transit = gameObject.AddComponent<CameraTransiting>();

        m_currentState = m_onBack;
    }

    // Use this for initialization
    void Start()
    {
        m_playerScript = m_player.GetComponent<Player>();
        m_cameraProtection = GetComponent<VariableCameraProtectFromWallClip>();

        m_initialCamPosition = m_cam.localPosition;
    }

    void Update()
    {
        float x = CrossPlatformInputManager.GetAxis("Mouse X");
        float y = CrossPlatformInputManager.GetAxis("Mouse Y");

        CameraStates previousState = m_currentState;
        if (m_currentState.OnUpdate(x, y, Time.deltaTime))
        {
            previousState.OnExit();
            m_currentState.OnEnter();
        }
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
                transitingCam.m_finalPosition = m_initialCamPosition;
                transitingCam.m_finalState = m_onBack;
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

}
