using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class MainCam : MonoBehaviour {

    [SerializeField] private float m_moveSpeed = 1f;
    [SerializeField] private float m_rotateSpeed = 1f;
    [Range(0f, 10f)] [SerializeField] private float m_turnSpeed = 1.5f;
    [SerializeField] private float m_maxReturnTime = 1f;
    [SerializeField] private float m_tiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    [SerializeField] private float m_tiltMin = 45f;

    float m_lookAngle;
    float m_tiltAngle;
    float m_returnTime;

    GameObject m_player;
    Player m_playerScript;
    Transform m_model;
    Transform m_pivot;
    Vector3 m_pivotEulers;
    Transform m_cam;

    Quaternion m_playerRotation;

    void Awake()
    {
        m_player = GameObject.Find("Player");
        m_model = m_player.transform.FindChild("Model");
        m_pivot = transform.FindChild("Pivot");
        m_pivotEulers = m_pivot.localRotation.eulerAngles;
        m_cam = transform.FindChild("Main Camera");
    }

    // Use this for initialization
    void Start ()
    {
        m_lookAngle = 0.0f;
        m_tiltAngle = 0.0f;
        m_returnTime = 0.0f;

        m_playerScript = m_player.GetComponent<Player>();
    }
	
	void Update ()
    {
        FollowTarget(Time.deltaTime);
        CameraRotation(Time.deltaTime);
    }

    void FollowTarget(float deltaTime)
    {
        transform.position = Vector3.Lerp(transform.position, m_player.transform.position, deltaTime * m_moveSpeed);
    }

    public void RotateOnTarget(float deltaTime)
    {
        transform.rotation = Quaternion.Lerp(transform.rotation, m_player.transform.rotation, deltaTime * m_rotateSpeed);
    }

    void CameraRotation(float deltaTime)
    {
        float x = CrossPlatformInputManager.GetAxis("Mouse X");
        float y = CrossPlatformInputManager.GetAxis("Mouse Y");

        if (m_playerScript.m_playerStopped && x == 0 && y == 0)
            m_returnTime += deltaTime;
        else
            m_returnTime = 0.0f;

        if (m_returnTime > m_maxReturnTime)
        {
            m_lookAngle = m_model.localRotation.eulerAngles.y;
            m_tiltAngle = 0.0f;

            Quaternion targetRotation = Quaternion.Euler(m_lookAngle * Vector3.up);
            Quaternion tiltRotation = Quaternion.Euler(m_tiltAngle, m_pivotEulers.y, m_pivotEulers.z);

            m_pivot.localRotation = Quaternion.Lerp(m_pivot.localRotation, targetRotation * tiltRotation, Time.deltaTime * m_moveSpeed);
        }
        else
        {
            m_lookAngle += x * m_turnSpeed;
            m_tiltAngle -= y * m_turnSpeed;
            m_tiltAngle = Mathf.Clamp(m_tiltAngle, -m_tiltMin, m_tiltMax);

            Quaternion targetRotation = Quaternion.Euler(m_lookAngle * Vector3.up);
            Quaternion tiltRotation = Quaternion.Euler(m_tiltAngle, m_pivotEulers.y, m_pivotEulers.z);

            m_pivot.localRotation = targetRotation * tiltRotation;
        }
        
    }
}
