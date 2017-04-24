using System.Collections;
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
    bool m_changeGravity;
    bool m_aimObject;
    bool m_throwObject;
    bool m_returnCam;
    bool m_negatePlayerInput;

    //Variables regarding player state
    public PlayerStates m_currentState;
    public PlayerStates m_grounded;
    public PlayerStates m_onAir;
    public PlayerStates m_throwing;
    public PlayerStates m_floating;
    public PlayerStates m_changing;

    //Variables regarding player movement
    Transform m_modelTransform;
    public bool m_freezeMovement;
    public VariableCam m_camController;
    public bool m_rotationFollowPlayer;
    public bool m_playerStopped = false;
    public Vector3 m_offset = Vector3.zero;
    public Vector3 m_lastMovement = Vector3.zero;
    public string m_tagGround = "";
    public float m_timeSlide = 0.2f;
    public float m_slideSpeed = 2.0f;
    private float m_timeSliding = 0.0f;

    //Variables regarding player's change of gravity
    public float m_gravityRange = 10.0f;
    public PlayerGravity m_playerGravity;
    public float m_maxTimeFloating = 30.0f;
    public float m_maxTimeChanging = 1.0f;
    public bool m_reachedGround = true;
    public bool m_changeButtonReleased = true;
    public float m_floatingHeight = 1.0f;

    //Variables regarding player's throw of objects
    public bool m_incresePowerWithTime = false;
    public float m_objectDetectionRadius = 1.5f;
    public float m_throwDetectionRange = 20.0f;
    public float m_maxTimeThrowing = 3.0f;
    public float m_throwStrengthPerSecond = 1.0f;
    public float m_throwStrengthOnce = 20.0f;
    public float m_objectsFloatingHeight = 1.0f;
    public float m_objectsRisingTime = 1.0f;
    public bool m_throwButtonReleased = true;
    public int m_maxNumberObjects = 1;

    //Variables reagarding player's helth and oxigen
    public float m_maxOxigen = 240;
    public float m_oxigen = 240;

	//Effects
	public GameObject m_jumpClouds;
	public GameObject m_runClouds;

    public Dictionary<string, TargetDetector> m_targetsDetectors;
    GameObject m_detectorsEmpty;
    float m_inputSpeed;
    float m_runSpeed;

    public override void Awake()
    {
        m_grounded = gameObject.AddComponent<PlayerGrounded>();
        m_onAir = gameObject.AddComponent<PlayerOnAir>();
        m_floating = gameObject.AddComponent<PlayerFloating>();
        m_changing = gameObject.AddComponent<PlayerChanging>();
        m_throwing = gameObject.AddComponent<PlayerThrowing>();

        m_currentState = m_onAir;

        if (!(m_playerInput = GetComponent<PlayerController>()))
            m_playerInput = gameObject.AddComponent<PlayerController>();
        if (!(m_playerGravity = GetComponent<PlayerGravity>()))        
            m_playerGravity = gameObject.AddComponent<PlayerGravity>();

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();

        GameObject cameraFree = GameObject.Find("MainCameraRig");
        if (cameraFree)
            m_camController = cameraFree.GetComponent<VariableCam>();
        m_rotationFollowPlayer = true;

        m_detectorsEmpty = GameObject.Find("TargetDetectors");
        if (!m_detectorsEmpty)
        {
            m_detectorsEmpty = new GameObject("TargetDetectors");
            m_detectorsEmpty.transform.parent = transform;
            m_detectorsEmpty.transform.localPosition = Vector3.zero;
        }

        m_targetsDetectors = new Dictionary<string, TargetDetector>();

        base.Awake();
    }

    // Use this for initialization
    public override void Start ()
    { 
        m_modelTransform = transform.FindChild("Model");

        m_freezeMovement = false;
        m_negatePlayerInput = false;

        base.Start();

        if (m_objectsRisingTime > m_maxTimeThrowing)
            m_objectsRisingTime = m_maxTimeThrowing;

        m_oxigen = m_maxOxigen;
        HUDManager.SetMaxEnergyValue(m_maxHealth);

        m_runSpeed = 2 * m_moveSpeed;
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
            m_playerInput.GetButtons(ref m_jumping, ref m_changeGravity, ref m_aimObject, ref m_throwObject, ref m_returnCam);
        }

        if (!m_changeGravity)
            m_changeButtonReleased = true;

        if (!m_aimObject)
            m_throwButtonReleased = true;

        m_playerStopped = false;

        m_inputSpeed = Mathf.Abs(m_axisHorizontal) + Mathf.Abs(m_axisVertical);

        DebugHits();

        PlayerStates previousState = m_currentState;
		if (m_currentState.OnUpdate(m_axisHorizontal, m_axisVertical, m_jumping, m_changeGravity, m_aimObject, m_throwObject, Time.deltaTime))
		{
			previousState.OnExit();
			m_currentState.OnEnter();
		}

        UpdateAnimator();

        if (m_camController)
        {
            m_camController.OnUpdate(m_camHorizontal, m_camVertical, m_returnCam, Time.deltaTime);
            if (m_rotationFollowPlayer)
                m_camController.RotateOnTarget(Time.fixedDeltaTime);
        }

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

        m_oxigen -= Time.fixedDeltaTime;
        if (m_oxigen <= 0.0f)
        {
            m_damage.m_damage = (int)m_health + 1;
            m_damage.m_recive = true;
            m_damage.m_respawn = true;
        }
        HUDManager.ChangeOxigen(m_oxigen / m_maxOxigen);
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

            m_rigidBody.MovePosition(transform.position + m_offset + movement * speed * timeStep);
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

    void UpdateAnimator()
    {
        m_animator.SetFloat("HorizontalVelocity", m_inputSpeed);
        m_animator.SetFloat("AirVelocity", Vector3.Dot(m_rigidBody.velocity,transform.up));
        m_animator.SetBool("Grounded", m_isGrounded);
        m_animator.SetBool("Jump", m_isJumping);
        m_animator.SetBool("Throwing", m_currentState == m_throwing);
    }

    public void SetFloatingPoint(float height)
    {
        PlayerFloating floating = (PlayerFloating)m_floating;

        floating.m_floatingPoint = transform.position + transform.up * height;

        m_groundCheckDistance = 0.1f;
    }

    void OnCollisionEnter(Collision col)
	{
        m_damage.m_force = -col.relativeVelocity * 10.0f;

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
			if (col.transform.GetComponentInParent<Enemy> ().m_animator.GetCurrentAnimatorStateInfo (0).IsName ("Attack"))
			{
				base.m_damage.m_recive = true;
				base.m_damage.m_damage = 20;
			}
		}

        int harmfulObject = LayerMask.NameToLayer("HarmfulObject");
        if(col.collider.gameObject.layer == harmfulObject)
        {
            base.m_damage.m_recive = true;
            base.m_damage.m_damage = 20;
        }
    }

    void ResetInput()
    {
        m_axisHorizontal = 0.0f;
        m_axisVertical = 0.0f;
        m_camHorizontal = 0.0f;
        m_camVertical = 0.0f;
        m_jumping = false;
        m_changeGravity = false;
        m_aimObject = false;
        m_throwObject = false;
        m_returnCam = false;
    }

    void DebugHits()
    {
        bool hit = Input.GetKey("8");
        bool dead = Input.GetKey("9");

        if (hit)
        {
            m_damage.m_force = -m_modelTransform.forward * 3.0f;
            base.m_damage.m_recive = true;
            base.m_damage.m_damage = 20;
        }
        


    }
}
