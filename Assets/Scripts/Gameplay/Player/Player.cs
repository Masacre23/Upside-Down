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
    bool m_changeGravityButtonUp = true;

    public bool m_negatePlayerInput = false;

    public bool m_savedPlayerInput = false;
    public bool m_paused = false;
    public bool m_justUnpaused = false;
    public bool m_negateJump = false;

    //Variables regarding player state
    public PlayerStates m_currentState;
    [HideInInspector] public PlayerStates m_grounded;
    [HideInInspector] public PlayerStates m_onAir;
    [HideInInspector] public PlayerStates m_aimToThrow;
    [HideInInspector] public PlayerStates m_floating;
    [HideInInspector] public PlayerStates m_changing;

    //Variables regarding player damage state
    public PlayerDamageStates m_playerDamageState;
    [HideInInspector] public PlayerDamageStates m_vulnerable;
    [HideInInspector] public PlayerDamageStates m_invulnerable;
    [HideInInspector] public PlayerDamageStates m_receivingDamage;
    [HideInInspector] public PlayerDamageStates m_deadState;
    [HideInInspector] public DamageData m_damageData;
    [HideInInspector] public PlayerRespawn m_playerRespawn;
    public Transform m_checkPoint;

    [HideInInspector] public bool m_damageForceToApply = false;
    [HideInInspector] public Vector3 m_damageForce;

    //General Info variables
    public Transform m_modelTransform;
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
    [HideInInspector] public Vector3 m_jumpDirection = Vector3.zero;
    private bool m_hasJumped = false;
    private Vector3 m_jumpVector;
    private Vector3 m_jumpMovement;

    //Variables regarding player's change of gravity
    public float m_gravityRange = 10.0f;
    public bool m_reachedGround = true;
    public MarkObject m_markedTarget = null;
    public bool m_markAimedObject = false;
    [HideInInspector] public TargetDetectorByTag m_gravityTargets;

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

    public SoundEffects m_soundEffects;

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

        m_vulnerable = gameObject.GetComponent<PlayerVulnerable>();
        if (!m_vulnerable)
            m_vulnerable = gameObject.AddComponent<PlayerVulnerable>();
        m_invulnerable = gameObject.GetComponent<PlayerInvulnerable>();
        if (!m_invulnerable)
            m_invulnerable = gameObject.AddComponent<PlayerInvulnerable>();
        m_receivingDamage = gameObject.GetComponent<PlayerReceivingDamage>();
        if (!m_receivingDamage)
            m_receivingDamage = gameObject.AddComponent<PlayerReceivingDamage>();
        m_deadState = gameObject.GetComponent<PlayerDead>();
        if (!m_deadState)
            m_deadState = gameObject.AddComponent<PlayerDead>();
        m_playerDamageState = m_vulnerable;
        m_damageData = new DamageData();
        m_playerRespawn = gameObject.GetComponent<PlayerRespawn>();

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

        GameObject detectorsEmpty = GameObject.Find("TargetDetectors");
        if (!detectorsEmpty)
        {
            detectorsEmpty = new GameObject("TargetDetectors");
            detectorsEmpty.transform.parent = transform;
            detectorsEmpty.transform.localPosition = Vector3.zero;
        }

        m_gravityTargets = GetComponentInChildren<TargetDetectorByTag>();

        m_soundEffects = GetComponent<SoundEffects>();

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
        ManageInput();

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

        if (!m_markAimedObject)
            UnmarkTarget();

        this.transform.position = this.transform.position + m_rigidBodyTotal;
        m_rigidBodyTotal = Vector3.zero;

        if (m_hasJumped)
        {
            m_rigidBody.velocity = m_jumpVector;
            m_hasJumped = false;
        }

        PlayerDamageStates previousDamageState = m_playerDamageState;
        if (m_playerDamageState.OnUpdate(m_damageData))
        {
            previousDamageState.OnExit(m_damageData);
            m_playerDamageState.OnEnter(m_damageData);
        }
        m_damageData.ResetDamageData();

        if (m_justUnpaused)
        {
            m_paused = false;
            m_justUnpaused = false;
        }

        m_doubleJumping = false;
        ResetInput();
    }

    public void ChangeCurrentStateToOnAir()
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
        //if (m_oxigen.m_oxigen <= 0.0f)
        //{
        //    m_damageData.m_damage = (int)m_health + 1;
        //    m_damageData.m_recive = true;
        //}
        //m_rigidBody.MovePosition(transform.position + m_rigidBodyTotal);
        //m_rigidBodyTotal = Vector3.zero;
        if (m_damageForceToApply)
        {
            m_rigidBody.AddForce(m_damageForce, ForceMode.VelocityChange);
            m_damageForceToApply = false;
        }
    }

    //This function sets a marked target that the player is aiming
    public bool SetMarkedTarget(out RaycastHit target)
    {
        bool ret = false;
        int layer = 1 << LayerMask.NameToLayer("Terrain");
        if (Physics.SphereCast(m_camController.m_camRay, 1.0f, out target, m_gravityRange, layer))
        {
            if (target.collider.tag == "GravityWall")
            {
                MarkObject newMarked = target.transform.GetComponent<MarkObject>();
                if (newMarked && target.transform.gameObject != m_gravityOnCharacter.m_attractorGameObject)
                {
                    ret = true;
                    if (newMarked != m_markedTarget)
                    {
                        UnmarkTarget();
                        newMarked.BeginMarking();
                        m_markedTarget = newMarked;
                    }
                }
                else
                    UnmarkTarget();
            }
            else
                UnmarkTarget();
        }
        else
            UnmarkTarget();

        return ret;
    }

    //This function unmarks a target, if any
    public void UnmarkTarget()
    {
        if (m_markedTarget != null)
        {
            m_markedTarget.StopMarking();
            m_markedTarget = null;
        }
    }

    public void SwapGravity()
    {
        if (m_gravityOnCharacter.m_getAttractorOnFeet)
        {
            m_gravityOnCharacter.ReturnToPlanet();
            if (m_soundEffects != null)
                m_soundEffects.PlaySound("GravityChange");

        }
        else
        {
            m_gravityOnCharacter.ActivateAttractorOnFeet();
            if (m_soundEffects != null)
                m_soundEffects.PlaySound("NewGravity");
        }
    }

    //This function deals with the jump of the character
    //It mainly adds a velocity to the rigidbody in the direction of the gravity.
    public override void Jump(float inputHorizontal, float inputVertical)
    {
        Vector3 forward = Vector3.Cross(Camera.main.transform.right, transform.up);
        Vector3 movement = inputHorizontal * Camera.main.transform.right + inputVertical * forward;
        movement.Normalize();

        float speed = m_inputSpeed > 0.5 ? m_runSpeed : m_moveSpeed;

        if (movement != Vector3.zero)
            m_modelTransform.rotation = Quaternion.LookRotation(movement, transform.up);

        m_hasJumped = true;
        m_jumpVector = Vector3.zero;
        m_jumpMovement = Vector3.zero;

        //m_jumpVector = m_gravityOnCharacter.GetGravityVector() * m_jumpForceVertical;
        m_jumpVector = transform.up * m_jumpForceVertical;
        m_jumpMovement = movement.normalized * speed;
        m_jumpDirection = movement.normalized;
        m_isGrounded = false;
        m_isJumping = true;
        m_groundCheckDistance = 0.01f;
        if (m_soundEffects != null)
            m_soundEffects.PlaySound("Jump");
    }

    //This functions controls the character movement and the model orientation.
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
                m_modelTransform.rotation = Quaternion.Slerp(m_modelTransform.rotation, modelRotation, 20.0f * timeStep);
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
                        m_lastMovement = Vector3.zero;
                }
                else
                    m_lastMovement = Vector3.zero;
            }
        }
    }

    //This functions controls the character movement and the model orientation while on air
    public void MoveOnAir(float timeStep)
    {
        if (!m_freezeMovement)
        {
            Vector3 forward = Vector3.Cross(Camera.main.transform.right, transform.up);
            Vector3 movement = m_axisHorizontal * Camera.main.transform.right + m_axisVertical * forward;
            movement.Normalize();

            //We need to ignore input in the direction of the jump
            Vector3 finalDirection = movement;
            float forwardIntensity = Vector3.Dot(movement, m_jumpDirection);
            if (forwardIntensity > 0.0f)
                finalDirection -= Vector3.Dot(movement, m_jumpDirection) * m_jumpDirection;

            float speed = m_inputSpeed > 0.5 ? m_runSpeed : m_moveSpeed;

            m_rigidBodyTotal += m_offset + (finalDirection * speed + m_jumpMovement) * timeStep;
            m_offset = Vector3.zero;

            if (movement != Vector3.zero)
            {
                Quaternion modelRotation = Quaternion.LookRotation(movement, transform.up);
                m_modelTransform.rotation = Quaternion.Slerp(m_modelTransform.rotation, modelRotation, 20.0f * timeStep);
                m_lastMovement = movement;
                m_timeSliding = 0.0f;
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
                if (targetEnemy)
                    m_floatingObjects.SetTarget(targetHit.point);
                else
                    m_floatingObjects.UnsetTarget();

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
        m_damageData.m_force = -col.relativeVelocity.normalized * 2.0f;

        int harmfulTerrain = LayerMask.NameToLayer("HarmfulTerrain");
        if (col.collider.gameObject.layer == harmfulTerrain)
        {
            m_damageData.m_recive = true;
            m_damageData.m_damage = 20;
            m_damageData.m_respawn = true;
            m_negatePlayerInput = true;
        }

        int enemy = LayerMask.NameToLayer("Enemy");
        if (col.collider.gameObject.layer == enemy) 
		{
			//if (col.gameObject.GetComponent<Enemy> ().m_animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack")) 
			if (col.transform.GetComponentInParent<Enemy> ().m_animator.GetCurrentAnimatorStateInfo(0).IsName ("Attack"))
			{
                m_damageData.m_recive = true;
                m_damageData.m_damage = 20;
            }
		}

        int harmfulObject = LayerMask.NameToLayer("HarmfulObject");
        if(col.collider.gameObject.layer == harmfulObject)
        {
            MovingDamagingObject objectMoving = col.collider.GetComponent<MovingDamagingObject>();
            StaticDamageObject objectStatic = col.collider.GetComponent<StaticDamageObject>();
            if (objectMoving)
            {
                m_damageData.m_recive = true;
                m_damageData.m_damage = objectMoving.m_impactDamage;
                m_damageData.m_force = objectMoving.m_directionMovement * objectMoving.m_forceMultiplier;
            }
            else if (objectStatic)
            {
                m_damageData.m_recive = true;
                m_damageData.m_damage = objectStatic.m_impactDamage;
            }
            else if (col.relativeVelocity.magnitude > 2.0f)
            {
                m_damageData.m_recive = true;
                m_damageData.m_damage = 20;
            }
        }
    }

    private void ManageInput()
    {
        if (!m_negatePlayerInput && !m_paused)
        {
            m_playerInput.GetDirections(ref m_axisHorizontal, ref m_axisVertical, ref m_camHorizontal, ref m_camVertical);
            m_playerInput.GetButtons(ref m_jumping, ref m_pickObjects, ref m_aimGravity, ref m_changeGravity, ref m_aimObject, ref m_throwObjectButtonDown, ref m_returnCam);

            //if (m_negateJump)
            //    m_jumping = false;

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

            //if (m_changeGravityButtonUp)
            //{
            //    if (m_changeGravity)
            //        m_changeGravityButtonUp = false;
            //}
            //else
            //{
            //    if (m_changeGravity)
            //        m_changeGravity = false;
            //    else
            //        m_changeGravityButtonUp = true;
            //}
            //if (m_changeGravity)
            //    m_changeGravityButtonUp = false;
        }
        m_playerStopped = false;
        m_inputSpeed = Mathf.Abs(m_axisHorizontal) + Mathf.Abs(m_axisVertical);
    }

    private void ResetInput()
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
            m_damageData.m_recive = true;
            m_damageData.m_damage = 20;
            m_damageData.m_force = -m_modelTransform.forward * 2.0f;
        }
        else if (dead)
        {
            m_damageData.m_recive = true;
            m_damageData.m_damage = (int)m_health + 1;
        }
    }

    public void PausePlayer()
    {
        m_paused = true;
        ResetInput();
    }

    public void UnpausePlayer()
    {
        m_justUnpaused = true;
        ResetInput();
    }

    public void PlaySound(string name)
    {
        if (m_soundEffects != null)
            m_soundEffects.PlaySound(name);
    }

}
