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
    bool m_jumping;
    bool m_changeGravity;
    bool m_throwObject;

    //Variables regarding player state
    public PlayerStates m_currentState;
    public PlayerStates m_grounded;
    public PlayerStates m_onAir;
    public PlayerStates m_throwing;
    public PlayerStates m_floating;
    public PlayerStates m_changing;

    //Variables regarding player movement
    Transform m_modelTransform;
    public bool m_freezeMovementOnAir;
    public VariableCam m_mainCam;
    public bool m_rotationFollowPlayer;
    public bool m_playerStopped = false;

    //Variables regarding player's change of gravity
    public float m_gravityRange = 10.0f;
    public GameObject m_gravitationSphere;
    public PlayerGravity m_playerGravity;
    public float m_maxTimeFloating = 30.0f;
    public float m_maxTimeChanging = 1.0f;
    public bool m_reachedGround = true;
    public bool m_changeButtonReleased = true;
    public float m_floatingHeight = 1.0f;

    //Variables regarding player's throw of objects
    public float m_maxTimeThrowing = 30.0f;
    public float m_throwStrengthPerSecond = 1.0f;
    public float m_objectsFloatingHeight = 1.0f;
    public bool m_throwButtonReleased = true;

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
        m_gravitationSphere = GameObject.Find("GravSphere");
        m_gravitationSphere.transform.localPosition = Vector3.zero + Vector3.up * capsuleCollider.height / 2;
        m_gravitationSphere.SetActive(false);

        GameObject cameraFree = GameObject.Find("MainCameraRig");
        if (cameraFree)
            m_mainCam = cameraFree.GetComponent<VariableCam>();
        m_rotationFollowPlayer = true;

        base.Awake();
    }

    // Use this for initialization
    public override void Start ()
    { 
        m_modelTransform = transform.FindChild("Model");

        m_freezeMovementOnAir = false;

        base.Start();
	}


    // This method should control player movements
    // First, it should read input from PlayerController in Update, since we need input every frame
    public override void Update()
    {
        m_playerInput.GetDirections(ref m_axisHorizontal, ref m_axisVertical);
        m_playerInput.GetButtons(ref m_jumping, ref m_changeGravity, ref m_throwObject);

        if (!m_changeGravity)
            m_changeButtonReleased = true;

        if (!m_throwObject)
            m_throwButtonReleased = true;

        m_playerStopped = false;

		PlayerStates previousState = m_currentState;
		if (m_currentState.OnUpdate(m_axisHorizontal, m_axisVertical, m_jumping, m_changeGravity, m_throwObject, Time.fixedDeltaTime))
		{
			previousState.OnExit();
			m_currentState.OnEnter();
		}

		if (m_mainCam && m_rotationFollowPlayer)
			m_mainCam.RotateOnTarget(Time.fixedDeltaTime);

		//m_playerGravity.DrawRay();

		m_axisHorizontal = 0.0f;
		m_axisVertical = 0.0f;
		m_jumping = false;
		m_changeGravity = false;
		m_throwObject = false;
    }

    // Second, it should update player state regarding the current state & input
    // We use FixedUpdate when we need to deal with physics
    // We also clean the input only after a FixedUpdate, so we are sure we have at least one FixedUpdate with the correct input recieved in Update
    public override void FixedUpdate ()
    {
        base.FixedUpdate();
        HUDManager.ChangeEnergyValue(base.m_health / base.m_maxHealth);
    }

    //This functions controls the character movement and the model orientation.
    //TODO: Probably we will need to change this function when we have the character's animations.
    public void Move(float timeStep)
    {
        Vector3 forward = Vector3.Cross(Camera.main.transform.right, transform.up);
        Vector3 movement = m_axisHorizontal * Camera.main.transform.right + m_axisVertical * forward;
        movement.Normalize();

        m_rigidBody.MovePosition(transform.position + movement * m_moveSpeed * timeStep);

        if (movement != Vector3.zero)
        {
            Quaternion modelRotation = Quaternion.LookRotation(movement, transform.up);
            m_modelTransform.rotation = Quaternion.Slerp(m_modelTransform.rotation, modelRotation, 10.0f * timeStep);
        }
    }

    public void SetFloatingPoint(float height)
    {
        PlayerFloating floating = (PlayerFloating)m_floating;

        floating.m_floatingPoint = transform.position + transform.up * height;

        m_groundCheckDistance = 0.1f;
    }

    //public void Move(float timeStep)
    //{
    //    Vector3 forward = Camera.main.transform.up;
    //    Vector3 movement = m_axisHorizontal * Camera.main.transform.right + m_axisVertical * forward;
    //    movement.Normalize();

    //    m_rigidBody.MovePosition(transform.position + movement * m_moveSpeed * timeStep);

    //    if (movement != Vector3.zero)
    //    {
    //        Quaternion modelRotation = Quaternion.LookRotation(movement, transform.up);
    //        m_modelTransform.rotation = Quaternion.Lerp(m_modelTransform.rotation, modelRotation, 10.0f * timeStep);
    //    }
    //}

    void OnCollisionEnter(Collision col)
	{
        if(col.collider.tag == "Enemy")
        {
            base.m_damageRecive = true;
            base.m_damagePower = 20;
        }
    }
}
