using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//This is the main class for the player. It will control the player input and character movement.
//It inherits from Character.
public class NewPlayer : Character {

    //State machine
    private enum PlayerState
    {
        GROUNDED,
        ONAIR,
        FLOATING,
        CHANGING
    }

    //Variables regarding player input control
    NewPlayerController m_playerInput;
    float m_axisHorizontal;
    float m_axisVertical;
    bool m_jumping;
    bool m_changeGravity;
    bool m_throwObject;
    bool m_throwObjectLastInput;

    //Variables regarding player movement
    PlayerState m_state;
    Transform m_camTransform;
    Transform m_modelTransform;
    [SerializeField] float m_onAirMovementLoss = 0.5f;

    //Variables regarding player's change of gravity
    GameObject m_gravitationSphere;
    PlayerGravity m_playerGravity;
    Quaternion m_initialRotation;
    Quaternion m_finalRotation;
    float m_timeFloating;
    float m_maxTimeFloating = 2.0f;
    float m_maxTimeChanging = 1.0f;
    bool m_changeEnabled = true;

    //Variables regarding object's change of gravity

    // Use this for initialization
    public override void Start ()
    {
        m_playerInput = GetComponent<NewPlayerController>();
        m_playerGravity = GetComponent<PlayerGravity>();

        CapsuleCollider capsuleCollider = GetComponent<CapsuleCollider>();
        m_gravitationSphere = GameObject.Find("GravSphere");
        m_gravitationSphere.transform.localPosition = Vector3.zero + Vector3.up * capsuleCollider.height / 2;
        m_gravitationSphere.SetActive(false);

        m_camTransform = transform.FindChild("FreeLookCameraRig");
        m_modelTransform = transform.FindChild("Model");

        m_state = PlayerState.ONAIR;

        m_throwObjectLastInput = false;
        m_timeFloating = 0.0f;

        base.Start();
	}


    // This method should control player movements
    // First, it should read input from PlayerController in Update, since we need input every frame
    public void Update()
    {
        m_playerInput.GetDirections(ref m_axisHorizontal, ref m_axisVertical);
        m_playerInput.GetButtons(ref m_jumping, ref m_changeGravity, ref m_throwObject);
    }

    // Second, it should update player state regarding the current state & input
    // We use FixedUpdate when we need to deal with physics
    // We also clean the input only after a FixedUpdate, so we are sure we have at least one FixedUpdate with the correct input recieved in Update
    public override void FixedUpdate ()
    {
        switch (m_state)
        {
            case PlayerState.GROUNDED:
                if (m_throwObject != m_throwObjectLastInput)
                    m_gravitationSphere.SetActive(m_throwObject);
                if (m_throwObject)
                    m_playerGravity.ChangeObjectGravity();
                if (m_jumping)
                {
                    Jump();
                    m_state = PlayerState.ONAIR;
                }
                else
                {
                    UpdateUp();
                    Move();
                    if (!CheckGroundStatus())
                        m_state = PlayerState.ONAIR;
                }
                break;
            case PlayerState.ONAIR:
                if (m_changeGravity && m_changeEnabled)
                {
                    m_rigidBody.isKinematic = true;
                    m_state = PlayerState.FLOATING;
                    m_gravitationSphere.SetActive(true);
                    m_changeEnabled = false;
                }
                else
                {
                    if (m_throwObject != m_throwObjectLastInput)
                        m_gravitationSphere.SetActive(m_throwObject);
                    if (m_throwObject)
                        m_playerGravity.ChangeObjectGravity();
                    OnAir();
                    UpdateUp();
                    Move();
                    if (CheckGroundStatus())
                    {
                        m_state = PlayerState.GROUNDED;
                        m_changeEnabled = true;
                    }
                }
                break;
            case PlayerState.FLOATING:
                if (m_timeFloating > m_maxTimeFloating)
                {
                    m_rigidBody.isKinematic = false;
                    m_state = PlayerState.ONAIR;
                    m_gravitationSphere.SetActive(false);
                    m_timeFloating = 0.0f;
                }
                else
                {
                    m_timeFloating += Time.fixedDeltaTime;
                    if (!m_changeGravity)
                    {
                        if (m_playerGravity.ChangePlayerGravity())
                        {
                            m_state = PlayerState.CHANGING;
                            m_initialRotation = transform.rotation;
                            m_finalRotation = Quaternion.FromToRotation(transform.up, m_gravityOnCharacter.m_gravity) * transform.rotation;
                        }
                        else
                        {
                            m_rigidBody.isKinematic = false;
                            m_state = PlayerState.ONAIR;
                            m_gravitationSphere.SetActive(false);
                        }
                        m_timeFloating = 0.0f;
                    }
                }
                break;
            case PlayerState.CHANGING:
                m_timeFloating += Time.fixedDeltaTime;
                float perc = m_timeFloating / m_maxTimeChanging;
                transform.rotation = Quaternion.Lerp(m_initialRotation, m_finalRotation, perc);

                if (m_timeFloating > m_maxTimeChanging)
                {
                    m_rigidBody.isKinematic = false;
                    m_state = PlayerState.ONAIR;
                    m_gravitationSphere.SetActive(false);
                    m_timeFloating = 0.0f;
                }
                break;
            default:
                break;
        }

        m_camTransform.position = transform.position;

        m_playerGravity.DrawRay();

        m_axisHorizontal = 0.0f;
        m_axisVertical = 0.0f;
        m_jumping = false;
        m_changeGravity = false;
        m_throwObjectLastInput = m_throwObject;
        m_throwObject = false;
    }

    //This functions controls the character movement and the model orientation.
    //TODO: Probably we will need to change this function when we have the character's animations.
    private void Move()
    {
        float moveSpeed = m_moveSpeed;
        if (m_state == PlayerState.ONAIR)
            moveSpeed *= m_onAirMovementLoss;

        Vector3 forward = Vector3.Cross(Camera.main.transform.right, transform.up);
        Vector3 movement = m_axisHorizontal * Camera.main.transform.right + m_axisVertical * forward;
        movement.Normalize();

        transform.Translate(movement * moveSpeed * Time.fixedDeltaTime, Space.World);

        if (movement != Vector3.zero)
        {
            Quaternion modelRotation = Quaternion.LookRotation(movement, transform.up);
            m_modelTransform.rotation = Quaternion.Lerp(m_modelTransform.rotation, modelRotation, 10.0f * Time.fixedDeltaTime);
        }

    }
}
