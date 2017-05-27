﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the main class for the player. It will control the player input and character movement.
//It inherits from Character.
public class Player : Character
{

    //Variables regarding player input control
    PlayerController m_playerInput;
    float m_axisHorizontal;
    float m_axisVertical;
    float m_camHorizontal;
    float m_camVertical;
    bool m_jumping;
    bool m_pickObjects;
    bool m_aimGravity;
    bool m_changeGravity;
    bool m_aimObject;
    bool m_throwObjectButtonDown;
    bool m_returnCam;

    bool m_throwObjectButtonUp = true;

    public bool m_negatePlayerInput;

    //Variables regarding player state
    public PlayerStates m_currentState;
    [HideInInspector] public PlayerStates m_grounded;
    [HideInInspector] public PlayerStates m_onAir;
    [HideInInspector] public PlayerStates m_aimToThrow;
    [HideInInspector] public PlayerStates m_floating;
    [HideInInspector] public PlayerStates m_changing;

    //General Info variables
    Transform m_modelTransform;
    [HideInInspector] public VariableCam m_camController;
    [HideInInspector] public PlayerGravity m_playerGravity;

    //Variables regarding player movement
    public bool m_freezeMovement;
    public bool m_rotationFollowPlayer;
    public bool m_playerStopped = false;
    public Vector3 m_offset = Vector3.zero;
    public Vector3 m_lastMovement = Vector3.zero;
    public string m_tagGround = "";
    public float m_timeSlide = 0.2f;
    public float m_slideSpeed = 2.0f;
    private float m_timeSliding = 0.0f;
    private Vector3 m_rigidBodyTotal = Vector3.zero;
    [HideInInspector] public bool m_doubleJumping = false;

    //Variables regarding player's change of gravity
    public float m_gravityRange = 10.0f;
    public bool m_reachedGround = true;

    //Variables regarding player's throw of objects
    public float m_throwDetectionRange = 20.0f;
    public float m_throwForce = 20.0f;
    public float m_angleEnemyDetection = 30.0f;
    [HideInInspector] public Transform m_throwAimOrigin;

    //Variables regarding player picking up objects
    [HideInInspector] public FloatingAroundPlayer m_floatingObjects;

    //Variables regarding player's health and oxigen
    [HideInInspector] public OxigenPlayer m_oxigen;

	//Effects
	public GameObject m_jumpClouds;
	public GameObject m_runClouds;

    [HideInInspector] public Dictionary<string, TargetDetector> m_targetsDetectors;
    float m_inputSpeed;
    float m_runSpeed;

    public override void Awake()
    {
        m_grounded = gameObject.GetComponent<PlayerGrounded>();
        if (!m_grounded)
            m_grounded = gameObject.AddComponent<PlayerGrounded>();

        m_onAir = gameObject.GetComponent<PlayerOnAir>();
        if (!m_onAir)
            m_onAir = gameObject.AddComponent<PlayerOnAir>();

        m_floating = gameObject.GetComponent<PlayerFloating>();
        if (!m_floating)
            m_floating = gameObject.AddComponent<PlayerFloating>();

        m_changing = gameObject.GetComponent<PlayerChanging>();
        if (!m_changing)
            m_changing = gameObject.AddComponent<PlayerChanging>();

        m_aimToThrow = gameObject.GetComponent<PlayerThrowing>();
        if (!m_aimToThrow)
            m_aimToThrow = gameObject.AddComponent<PlayerThrowing>();

        m_currentState = m_onAir;

        if (!(m_playerInput = GetComponent<PlayerController>()))
            m_playerInput = gameObject.AddComponent<PlayerController>();
        if (!(m_playerGravity = GetComponent<PlayerGravity>()))        
            m_playerGravity = gameObject.AddComponent<PlayerGravity>();

        if (!(m_oxigen = GetComponent<OxigenPlayer>()))
            m_oxigen = gameObject.AddComponent<OxigenPlayer>();

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();

        GameObject cameraFree = GameObject.Find("MainCameraRig");
        if (cameraFree)
            m_camController = cameraFree.GetComponent<VariableCam>();
        m_rotationFollowPlayer = true;

        GameObject m_detectorsEmpty = GameObject.Find("TargetDetectors");
        if (!m_detectorsEmpty)
        {
            m_detectorsEmpty = new GameObject("TargetDetectors");
            m_detectorsEmpty.transform.parent = transform;
            m_detectorsEmpty.transform.localPosition = Vector3.zero;
        }

        m_targetsDetectors = new Dictionary<string, TargetDetector>();

        m_floatingObjects = GetComponentInChildren<FloatingAroundPlayer>();

        m_throwAimOrigin = GameObject.Find("ThrowAimRaycast").transform;

        base.Awake();
    }

    // Use this for initialization
    public override void Start ()
    { 
        m_modelTransform = transform.FindChild("Model");

        m_freezeMovement = false;
        m_negatePlayerInput = false;

        base.Start();

        HUDManager.SetMaxEnergyValue(m_maxHealth);

        m_runSpeed = 2 * m_moveSpeed;

        m_rigidBodyTotal = Vector3.zero;
    }

    public override void Restart()
    {
        m_currentState.OnExit();
        m_currentState = m_onAir;

        ResetInput();

        m_freezeMovement = false;
        m_negatePlayerInput = false;

        base.Restart();
    }

    // This method should control player movements
    // First, it should read input from PlayerController in Update, since we need input every frame
    public override void Update()
    {
        if (!m_negatePlayerInput)
        {
            m_playerInput.GetDirections(ref m_axisHorizontal, ref m_axisVertical, ref m_camHorizontal, ref m_camVertical);
            m_playerInput.GetButtons(ref m_jumping, ref m_pickObjects, ref m_aimGravity, ref m_changeGravity, ref m_aimObject, ref m_throwObjectButtonDown, ref m_returnCam);

            if (m_throwObjectButtonUp)
            {
                if (m_throwObjectButtonDown)
                    m_throwObjectButtonUp = false;
            }
            else
            {
                if (m_throwObjectButtonDown)
                    m_throwObjectButtonDown = false;
                else
                    m_throwObjectButtonUp = true;
            }

            if (m_throwObjectButtonDown)
                m_throwObjectButtonUp = false;
        }

        m_playerStopped = false;

        m_inputSpeed = Mathf.Abs(m_axisHorizontal) + Mathf.Abs(m_axisVertical);

        PlayerStates previousState = m_currentState;
		if (m_currentState.OnUpdate(m_axisHorizontal, m_axisVertical, m_jumping, m_pickObjects, m_aimGravity, m_changeGravity, m_aimObject, m_throwObjectButtonDown, Time.deltaTime))
		{
			previousState.OnExit();
			m_currentState.OnEnter();
		}

        m_modelTransform.rotation = Quaternion.FromToRotation(m_modelTransform.up, transform.up) * m_modelTransform.rotation;

        UpdateAnimator();

        if (m_camController)
        {
            m_camController.OnUpdate(m_camHorizontal, m_camVertical, m_returnCam, Time.deltaTime);
            if (m_rotationFollowPlayer)
                m_camController.RotateOnTarget(Time.fixedDeltaTime);
        }

        this.transform.position = this.transform.position + m_rigidBodyTotal;
        m_rigidBodyTotal = Vector3.zero;

        m_doubleJumping = false;
        ResetInput();
    }

    public void ChangeCurrntStateToOnAir()
    {
        PlayerStates previousState = m_currentState;
        m_currentState = m_onAir;
        previousState.OnExit();
        m_currentState.OnEnter();
    }

    // Second, it should update player state regarding the current state & input
    // We use FixedUpdate when we need to deal with physics
    // We also clean the input only after a FixedUpdate, so we are sure we have at least one FixedUpdate with the correct input recieved in Update
    public override void FixedUpdate ()
    {
        base.FixedUpdate();
        HUDManager.ChangeEnergyValue(base.m_health);
        if (m_oxigen.m_oxigen <= 0.0f)
        {
            m_damage.m_damage = (int)m_health + 1;
            m_damage.m_recive = true;
            //m_damage.m_respawn = true;
        }
        //m_rigidBody.MovePosition(transform.position + m_rigidBodyTotal);
        //m_rigidBodyTotal = Vector3.zero;
    }

    //This functions controls the character movement and the model orientation.
    //TODO: Probably we will need to change this function when we have the character's animations.
    public void Move(float timeStep)
    {
        if (!m_freezeMovement)
        {
            Vector3 forward = Vector3.Cross(Camera.main.transform.right, transform.up);
            Vector3 movement = m_axisHorizontal * Camera.main.transform.right + m_axisVertical * forward;
            movement.Normalize();

            float speed = m_inputSpeed > 0.5 ? m_runSpeed : m_moveSpeed;

            //m_rigidBody.MovePosition(transform.position + m_offset + movement * speed * timeStep);
            m_rigidBodyTotal += m_offset + movement * speed * timeStep;
            m_offset = Vector3.zero;

            if (movement != Vector3.zero)
            {
                Quaternion modelRotation = Quaternion.LookRotation(movement, transform.up);
                m_modelTransform.rotation = Quaternion.Slerp(m_modelTransform.rotation, modelRotation, 10.0f * timeStep);
                m_lastMovement = movement;
                m_timeSliding = 0.0f;
            }
            else
            {
                m_timeSliding += timeStep;
                if(m_tagGround == "Ice" && m_currentState == m_grounded)
                {
                    m_rigidBody.MovePosition(transform.position + m_lastMovement * m_slideSpeed * timeStep);
                    if (m_timeSliding >= m_timeSlide)
                    {
                        m_lastMovement = Vector3.zero;
                    }
                }
                else
                {
                    m_lastMovement = Vector3.zero;
                }
                
            }
        }
    }

    public void RotateModel(Vector3 forward)
    {
        Quaternion modelRotation = Quaternion.LookRotation(forward, m_modelTransform.up);
        m_modelTransform.rotation = modelRotation;
    }

    void UpdateAnimator()
    {
        m_animator.SetFloat("HorizontalVelocity", m_inputSpeed);
        m_animator.SetFloat("AirVelocity", Vector3.Dot(m_rigidBody.velocity,transform.up));
        m_animator.SetBool("Grounded", m_isGrounded);
        m_animator.SetBool("Jump", m_isJumping);
        m_animator.SetBool("DoubleJump", m_doubleJumping);
        m_animator.SetBool("Throwing", m_currentState == m_aimToThrow);
    }

    public void SetFloatingPoint(float height)
    {
        PlayerFloating floating = (PlayerFloating)m_floating;

        floating.m_floatingPoint = transform.position + transform.up * height;

        m_groundCheckDistance = 0.1f;
    }

    public void PickObjects()
    {
        if (m_floatingObjects.CanPickMoreObjects())
            m_floatingObjects.PickObjects(transform.position + transform.up * (m_capsuleHeight / 2));
    }

    public void ThrowObjectsThirdPerson(bool hasThrown)
    {
        if (m_floatingObjects.HasObjectsToThrow())
        {
            GameObject targetEnemy = FixingOnEnemy(m_throwAimOrigin, m_angleEnemyDetection);

            RaycastHit targetHit;
            bool hasTarget = false;
            if (targetEnemy != null)
            {
                Vector3 toEnemy = targetEnemy.transform.position - m_throwAimOrigin.position;
                hasTarget = Physics.Raycast(m_throwAimOrigin.position, toEnemy.normalized, out targetHit, m_throwDetectionRange);
            }
            else
            {
                hasTarget = Physics.Raycast(m_throwAimOrigin.position, m_throwAimOrigin.forward, out targetHit, m_throwDetectionRange);
                Debug.DrawRay(m_throwAimOrigin.position, m_throwAimOrigin.forward, Color.red);
            }

            if (hasTarget)
            {
                m_floatingObjects.SetTarget(targetHit.point);
                if (hasThrown)
                    m_floatingObjects.ThrowObjectToTarget(targetHit, m_throwAimOrigin, m_throwForce);
            }
            else
            {
                m_floatingObjects.UnsetTarget();
                if (hasThrown)
                    m_floatingObjects.ThrowObjectToDirection(m_throwAimOrigin, m_throwDetectionRange, m_throwForce);
            }
        }
        else
            m_floatingObjects.UnsetTarget();
    }

    public GameObject FixingOnEnemy(Transform origin, float angleDetection)
    {
        GameObject closestTarget = null;
        float closestDistance = 10000.0f;
        foreach (GameObject target in m_targetsDetectors["Enemy"].m_targets)
        {
            if (!m_floatingObjects.EnemyIsFloating(target))
            {
                Vector3 toTarget = target.transform.position - origin.position;
                float distance = toTarget.sqrMagnitude;
                float thisAngle = Vector3.Angle(origin.forward, toTarget);
                if (thisAngle < angleDetection && distance < closestDistance)
                {
                    closestDistance = distance;
                    closestTarget = target;
                }
            }
        }

        return closestTarget;
    }

    void OnCollisionEnter(Collision col)
	{
        //m_damage.m_force = -col.relativeVelocity * 10.0f;
        base.m_damage.m_force = -col.relativeVelocity.normalized * 2.0f;

        int harmfulTerrain = LayerMask.NameToLayer("HarmfulTerrain");
        if (col.collider.gameObject.layer == harmfulTerrain)
        {
            base.m_damage.m_recive = true;
            base.m_damage.m_damage = 20;
            base.m_damage.m_respawn = true;
            m_negatePlayerInput = true;
        }

        int enemy = LayerMask.NameToLayer("Enemy");
        if (col.collider.gameObject.layer == enemy) 
		{
			//if (col.gameObject.GetComponent<Enemy> ().m_animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) 
			if (col.transform.GetComponentInParent<Enemy> ().m_animator.GetCurrentAnimatorStateInfo(0).IsName ("Attack"))
			{
                base.m_damage.m_recive = true;
                base.m_damage.m_damage = 20;
            }
		}

        int harmfulObject = LayerMask.NameToLayer("HarmfulObject");
        if(col.collider.gameObject.layer == harmfulObject)
        {
            if (col.relativeVelocity.magnitude > 2.0f)
            {
                base.m_damage.m_recive = true;
                base.m_damage.m_damage = 20;
            }
        }
    }

    void ResetInput()
    {
        m_axisHorizontal = 0.0f;
        m_axisVertical = 0.0f;
        m_camHorizontal = 0.0f;
        m_camVertical = 0.0f;
        m_jumping = false;
        m_pickObjects = false;
        m_aimGravity = false;
        m_changeGravity = false;
        m_aimObject = false;
        m_throwObjectButtonDown = false;
        m_returnCam = false;
    }

    public void HitDebug()
    {
        bool hit = Input.GetKeyDown(KeyCode.Alpha8);
        bool dead = Input.GetKeyDown(KeyCode.Alpha9);

        if (hit)
        {
            base.m_damage.m_recive = true;
            base.m_damage.m_damage = 20;
            base.m_damage.m_force = -m_modelTransform.forward * 2.0f;
        }
        else if (dead)
        {
            base.m_damage.m_recive = true;
            base.m_damage.m_damage = (int)m_health + 1;
        }
    }

}
