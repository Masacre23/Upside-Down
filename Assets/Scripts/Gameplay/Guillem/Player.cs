using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(GravityOnGameObject))]
[RequireComponent(typeof(GravityChange))]
public class Player : MonoBehaviour
{
    public float m_MovingTurnSpeed = 360;
    public float m_StationaryTurnSpeed = 180;
    public float m_JumpPower = 12f;
    public float m_RunCycleLegOffset = 0.2f; //specific to the character in sample assets, will need to be modified to work with others
    public float m_MoveSpeedMultiplier = 1f;
    public float m_AnimSpeedMultiplier = 1f;
    public float m_GroundCheckDistance = 0.3f;

    Rigidbody m_Rigidbody;
    Animator m_Animator;
    CapsuleCollider m_Capsule;
    GravityOnGameObject m_GravityOnPlayer;
    GravityChange m_GravityChange;

    public bool m_IsGrounded;
    public float m_OrigGroundCheckDistance;
    const float k_Half = 0.5f;
    float m_TurnAmount;
    float m_ForwardAmount;
    public Vector3 m_GroundNormal;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;

    public bool m_DestinationReached = false;

    void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_GravityOnPlayer = GetComponent<GravityOnGameObject>();
        m_GravityChange = GetComponent<GravityChange>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationY | RigidbodyConstraints.FreezeRotationZ;
        m_OrigGroundCheckDistance = m_GroundCheckDistance;

    }

    public void Move(Vector3 move, bool jump, bool player_gravity, bool object_gravity)
    {
        // convert the world relative moveInput vector into a local-relative
        // turn amount and forward amount required to head in the desired
        // direction.
        if (move.magnitude > 1f)
            move.Normalize();
        move = transform.InverseTransformDirection(move);
        CheckGroundStatus();
        move = Vector3.ProjectOnPlane(move, m_GroundNormal);
        m_TurnAmount = Mathf.Atan2(move.x, move.z);
        m_ForwardAmount = move.z;

        ApplyExtraTurnRotation();

        Vector3 origin_ray = transform.position + (transform.up * m_CapsuleHeight / 2);
        m_GravityChange.DrawRay(origin_ray);
        if (player_gravity)
        {
            if (m_GravityChange.ChangePlayerGravity(origin_ray) && m_GravityChange.m_ZeroSpeedOnChange)
                m_Rigidbody.velocity = Vector3.zero;
        }

        if (object_gravity)
        {
            m_GravityChange.ChangeObjectsGravity(origin_ray);
        }

        Vector3 current_gravity = m_GravityOnPlayer.m_Gravity;
        if (current_gravity.normalized != transform.up)
        {
            Quaternion targetRotation = Quaternion.FromToRotation(transform.up, current_gravity) * transform.rotation;
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, m_GravityChange.m_ChangeGravityRotationSpeed * Time.deltaTime);
        }

        // control and velocity handling is different when grounded and airborne:
        if (m_IsGrounded)
        {
            HandleGroundedMovement(jump);
        }
        else
        {
            HandleAirborneMovement();
        }
    }

    void HandleAirborneMovement()
    {
        m_GroundCheckDistance = Vector3.Dot(m_Rigidbody.velocity, transform.up) < 0 ? m_OrigGroundCheckDistance : 0.01f;
    }

    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        if (jump && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        {
            // jump!
            m_Rigidbody.velocity += m_GravityOnPlayer.m_Gravity * m_JumpPower;
            m_IsGrounded = false;
            m_Animator.applyRootMotion = false;
            m_GroundCheckDistance = 0.1f;
        }
    }

    void ApplyExtraTurnRotation()
    {
        // help the character turn faster (this is in addition to root rotation in the animation)
        float turnSpeed = Mathf.Lerp(m_StationaryTurnSpeed, m_MovingTurnSpeed, m_ForwardAmount);
        transform.Rotate(0, m_TurnAmount * turnSpeed * Time.deltaTime, 0);
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (transform.up * 0.1f), transform.position + (transform.up * 0.1f) + (-transform.up * m_GroundCheckDistance), Color.magenta);
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, m_GroundCheckDistance))
        {
            m_GravityChange.GravityOnFeet(hitInfo);
            m_GroundNormal = transform.InverseTransformVector(hitInfo.normal);
            m_IsGrounded = true;
            m_Animator.applyRootMotion = true;
            
            if (!m_DestinationReached)
                m_DestinationReached = true;
        }
        else
        {
            m_IsGrounded = false;
            m_GroundNormal = Vector3.up;
            m_Animator.applyRootMotion = false;
        }
    }
}
