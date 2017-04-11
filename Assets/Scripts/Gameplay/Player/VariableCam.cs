using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class VariableCam : MonoBehaviour
{
    [SerializeField] private float m_moveSpeed = 1f;
    [SerializeField] private float m_rotateSpeed = 1f;
    [Range(0f, 10f)] [SerializeField] private float m_turnSpeed = 1.5f;
    [SerializeField] private float m_maxReturnTime = 1f;
    [SerializeField] private float m_tiltMax = 75f;                       // The maximum value of the x axis rotation of the pivot.
    [SerializeField] private float m_tiltMin = 45f;

    float m_lookAngle;
    float m_tiltAngle;
    float m_returnTime;

    Transform m_player;
    Transform m_pivot;
    Vector3 m_pivotEulers;
    Transform m_cam;

    Quaternion m_playerRotation;

    void Awake()
    {
        m_player = GameObject.Find("Player").transform;
        m_pivot = transform.FindChild("Pivot");
        m_pivotEulers = m_pivot.localRotation.eulerAngles;
        m_cam = m_pivot.FindChild("Main Camera");
    }

    // Use this for initialization
    void Start()
    {
        transform.position = m_player.position;
        transform.rotation = m_player.rotation;
    }

    void Update()
    {
        FollowTarget(Time.deltaTime);
        m_cam.LookAt(m_player);
        //CameraRotation();
    }

    void FollowTarget(float deltaTime)
    {
        //transform.position = Vector3.Lerp(transform.position, m_player.position, deltaTime * m_moveSpeed);
        transform.position = m_player.position;
        transform.rotation = m_player.rotation;
    }

    public void RotateOnTarget(float deltaTime)
    {
        //transform.rotation = Quaternion.Lerp(transform.rotation, m_player.rotation, deltaTime * m_rotateSpeed);
    }

    void CameraRotation()
    {
        float x = CrossPlatformInputManager.GetAxis("Mouse X");
        float y = CrossPlatformInputManager.GetAxis("Mouse Y");


    }

    void CameraAiming()
    {
        float x = CrossPlatformInputManager.GetAxis("Mouse X");
        float y = CrossPlatformInputManager.GetAxis("Mouse Y");

        m_lookAngle += x * m_turnSpeed;
        m_tiltAngle -= y * m_turnSpeed;
        m_tiltAngle = Mathf.Clamp(m_tiltAngle, -m_tiltMin, m_tiltMax);

        Quaternion targetRotation = Quaternion.Euler(m_lookAngle * Vector3.up);
        Quaternion tiltRotation = Quaternion.Euler(m_tiltAngle, m_pivotEulers.y, m_pivotEulers.z);

        m_pivot.localRotation = targetRotation * tiltRotation;
    }
}
