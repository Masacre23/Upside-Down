using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the main class for the player. It will control the player input and character movement.
//It inherits from Character.
public class Player : Character
{

    public bool m_fixedCam = false;
    //Variables regarding player input control
    PlayerController m_playerInput;
    float m_axisHorizontal;
    float m_axisVertical;
    float m_camHorizontal;
    float m_camVertical;
    bool m_jumping;
    bool m_pickObjects;
    bool m_changeGravity;
    bool m_aimObject;
    bool m_throwObjectButtonDown;
    bool m_returnCam;

    bool m_throwObjectButtonUp = true;
    bool m_throwAnimation = false;

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
    [HideInInspector] public Vector3 m_jumpMovement;
    private bool m_hasJumped = false;
    private Vector3 m_jumpVector;

    //Variables regarding player's change of gravity
    public float m_gravityRange = 3.0f;
    public bool m_reachedGround = true;
    public MarkObject m_markedTarget = null;
    public bool m_markAimedObject = false;
    [HideInInspector] public TargetDetectorByTag m_gravityTargets;

    //Variables regarding player's throw of objects
    public float m_throwDetectionRange = 20.0f;
    public float m_throwForce = 20.0f;
    public float m_angleEnemyDetection = 30.0f;
    [HideInInspector] public Transform m_throwAimOrigin;
    [HideInInspector] public Transform m_lowAimOrigin;
    [HideInInspector] public Transform m_highAimOrigin;

    //Variables regarding player picking up objects
    [HideInInspector] public FloatingAroundPlayer m_floatingObjects;

    //Variables regarding player's health and oxigen
    [HideInInspector] public OxigenPlayer m_oxigen;

	//Effects
	public GameObject m_jumpClouds;
	public GameObject m_runClouds;
    public Transform m_smoke;

    [HideInInspector] public Dictionary<string, TargetDetector> m_targetsDetectors;
    float m_inputSpeed;
    float m_runSpeed;

    public SoundEffects m_soundEffects;
    private bool m_InIce;

    public override void Awake()
    {
        m_grounded = gameObject.GetComponent<PlayerGrounded>();
        if (!m_grounded)
            m_grounded = gameObject.AddComponent<PlayerGrounded>();
        m_onAir = gameObject.GetComponent<PlayerOnAir>();
        if (!m_onAir)
            m_onAir = gameObject.AddComponent<PlayerOnAir>();
        //m_floating = gameObject.GetComponent<PlayerFloating>();
        //if (!m_floating)
        //    m_floating = gameObject.AddComponent<PlayerFloating>();
        //m_changing = gameObject.GetComponent<PlayerChanging>();
        //if (!m_changing)
        //    m_changing = gameObject.AddComponent<PlayerChanging>();
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
        m_lowAimOrigin = GameObject.Find("LowAimRaycast").transform;
        m_highAimOrigin = GameObject.Find("HighAimRaycast").transform;
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

        m_soundEffects = GetComponent<SoundEffects>();

        base.Restart();
    }

    // This method should control player movements
    // First, it should read input from PlayerController in Update, since we need input every frame
    public override void Update()
    {
        ManageInput();

        PlayerStates previousState = m_currentState;
		if (m_currentState.OnUpdate(m_axisHorizontal, m_axisVertical, m_jumping, m_pickObjects, m_changeGravity, m_aimObject, m_throwObjectButtonDown, Time.deltaTime))
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
            {
                //m_camController.FollowTarget(Time.deltaTime);
                m_camController.RotateOnTarget(Time.deltaTime);
            }   
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

    //This function looks for a gravity wall in front of the player
    //It marks the target if someone is found (unmarking previous marked objects)
    //If no one is found, it unmarks previous objects
    public bool GetGravityChangeTarget(out RaycastHit target)
    {
        bool ret = false;

        //First look for a target in front of character (ThrowAimRaycast), 
        // if not found look for one below (LowAimRaycast)
        // if not found look for one up (HighAimRaycast)

        ret = GetGravityWall(m_throwAimOrigin, out target);
        if (!ret)
            ret = GetGravityWall(m_lowAimOrigin, out target);
        if (!ret)
            ret = GetGravityWall(m_highAimOrigin, out target);

        if (!ret)
            UnmarkTarget();

        return ret;
    }

    //This gets only one gravity wall, casting a raycast from and forward of Origin
    private bool GetGravityWall(Transform origin, out RaycastHit target)
    {
        bool ret = false;

        int layerMask = 1 << LayerMask.NameToLayer("Terrain");
        if (Physics.Raycast(origin.position, origin.forward, out target, m_gravityRange, layerMask))
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
            }
        }

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

    //This functions computes the direction of the jump from the player input
    //Then calls the jump method in the computed direction
    public override void Jump(float inputHorizontal, float inputVertical)
    {
        Vector3 forward = GetDirectionForward();
        Vector3 movement = inputHorizontal * GetDirectionRight() + inputVertical * forward;
        movement.Normalize();

        JumpInDirection(movement, m_inputSpeed);
    }

    //This function deals with the jump of the character
    //It mainly adds a velocity to the rigidbody in the direction of the gravity.
    public void JumpInDirection (Vector3 movement, float inputIntensity)
    {
        float speed = GetSpeedFromInput(inputIntensity);

        if (movement != Vector3.zero)
            m_modelTransform.rotation = Quaternion.LookRotation(movement, transform.up);

        m_hasJumped = true;
        m_jumpVector = Vector3.zero;
        m_jumpMovement = Vector3.zero;

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
            Vector3 forward = GetDirectionForward();
            Vector3 movement = m_axisHorizontal * GetDirectionRight() + m_axisVertical * forward;
            movement.Normalize();

            float speed = GetSpeedFromInput(m_inputSpeed);

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
            Vector3 forward = GetDirectionForward();
            Vector3 movement = m_axisHorizontal * GetDirectionRight() + m_axisVertical * forward;
            movement.Normalize();

            //We need to ignore input in the direction of the jump
            Vector3 finalDirection = movement;
            float forwardIntensity = Vector3.Dot(movement, m_jumpDirection);
            if (forwardIntensity > 0.0f)
                finalDirection -= Vector3.Dot(movement, m_jumpDirection) * m_jumpDirection;

            float speed = GetSpeedFromInput(m_inputSpeed);

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
        m_animator.SetBool("Throwing", m_throwAnimation || (m_currentState == m_aimToThrow));
        m_throwAnimation = false;
    }

    public void PickObjects()
    {
        if (m_floatingObjects.CanPickMoreObjects())
        {
            m_floatingObjects.PickObjects(transform.position + transform.up * (m_capsuleHeight / 2));
        }
            
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
                {
                    m_floatingObjects.ThrowObjectToTarget(targetHit, m_throwAimOrigin, m_throwForce);
                    m_throwAnimation = true;
                }
            }
            else
            {
                m_floatingObjects.UnsetTarget();
                if (hasThrown)
                {
                    m_floatingObjects.ThrowObjectToDirection(m_throwAimOrigin, m_throwDetectionRange, m_throwForce);
                    m_throwAnimation = true;
                }
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

                RaycastHit hit;
                bool hasHit = Physics.Raycast(origin.position, toTarget.normalized, out hit, toTarget.magnitude);
                //float thisAngle = Vector3.Angle(origin.forward, toTarget);
                //if (thisAngle < angleDetection && distance < closestDistance)
                if (distance < closestDistance && hasHit && hit.transform.gameObject == target)
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
			if (col.gameObject.tag != "FlyingEnemy") //If is snail
			{ 
				if (col.transform.GetComponentInParent<Enemy> ().m_animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack"))
                {
					if (m_soundEffects != null) {
                        m_soundEffects.PlaySound ("Scream");
					}
					m_damageData.m_recive = true;
					m_damageData.m_damage = 20;
				}
			} 
			else 
			{
                if(m_soundEffects != null)
                    m_soundEffects.PlaySound("Scream");
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

    private void OnCollisionStay(Collision collision)
    {
        int terrain = LayerMask.NameToLayer("Floor");
        if (collision.collider.gameObject.layer == terrain)
        {
            //if(collision.collider.tag == "Ice")
            //{
                m_InIce = true;
            //}else
            //{
            //    m_InIce = false;
            //} TODO: Esteban.
        }

    }

    private void ManageInput()
    {
        if (!m_negatePlayerInput && !m_paused)
        {
            m_playerInput.GetDirections(ref m_axisHorizontal, ref m_axisVertical, ref m_camHorizontal, ref m_camVertical);
            m_playerInput.GetButtons(ref m_jumping, ref m_pickObjects, ref m_changeGravity, ref m_aimObject, ref m_throwObjectButtonDown, ref m_returnCam);

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

    public void PlayFootStep(string right)
    {
        string footstep = right == "Right" ? "FootStepRight" : "FootStepLeft";
        if (!m_InIce)
        {
            footstep += "Land";
        }
        if (m_soundEffects != null)
            m_soundEffects.PlaySound(footstep);
    }

    public void PlaySound(string name)
    {
        if (m_soundEffects != null)
            m_soundEffects.PlaySound(name);
    }

    private Vector3 GetDirectionRight()
    {
        //return Camera.main.transform.right;
        if (m_fixedCam)
        {
            return Camera.main.transform.right - Vector3.Dot(Camera.main.transform.right, transform.up) * transform.up;
        }  
        else
        {
            Vector3 forwardProjection = Camera.main.transform.forward - Vector3.Dot(Camera.main.transform.forward, transform.up) * transform.up;

            //return Vector3.Cross(transform.up, Camera.main.transform.forward).normalized;
            //return Camera.main.transform.right;
            return Camera.main.transform.right - Vector3.Dot(Camera.main.transform.right, transform.up) * transform.up;
        }
            
    }

    private Vector3 GetDirectionForward()
    {
        if (m_fixedCam)
        {
            return Camera.main.transform.forward - Vector3.Dot(Camera.main.transform.forward, transform.up) * transform.up;
        }
        else
        {
            //return Vector3.Cross(Camera.main.transform.right, transform.up).normalized;
            return Camera.main.transform.forward - Vector3.Dot(Camera.main.transform.forward, transform.up) * transform.up;
        }
            

    }

    private float GetSpeedFromInput(float inputIntensity)
    {
        return inputIntensity > 0.5 ? m_runSpeed : m_moveSpeed;
    }
}
