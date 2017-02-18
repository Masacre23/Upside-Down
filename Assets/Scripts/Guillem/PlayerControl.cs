using System;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Player))]
public class PlayerControl : MonoBehaviour
{
    private Player m_Character; // A reference to the ThirdPersonCharacter on the object
    private Transform m_Cam;                  // A reference to the main camera in the scenes transform
    private Vector3 m_CamForward;             // The current forward direction of the camera
    private Vector3 m_Move;

    private bool m_Jump;                      // the world-relative desired move direction, calculated from the camForward and user input.
    private bool m_ChangeCharacterGravity;
    private bool m_ChangeObjectGravity;

    [SerializeField]
    bool m_MovementRelativeToCam = false;

    private void Start()
    {
        // get the transform of the main camera
        if (Camera.main != null)
        {
            m_Cam = Camera.main.transform;
        }
        else
        {
            Debug.LogWarning(
                "Warning: no main camera found. Third person character needs a Camera tagged \"MainCamera\", for camera-relative controls.", gameObject);
            m_MovementRelativeToCam = false;
            // we use self-relative controls in this case, which probably isn't what the user wants, but hey, we warned them!
        }

        // get the third person character ( this should never be null due to require component )
        m_Character = GetComponent<Player>();
    }


    private void Update()
    {
        if (!m_Jump)
            m_Jump = CrossPlatformInputManager.GetButtonDown("Jump");
        if (!m_ChangeCharacterGravity)
            m_ChangeCharacterGravity = CrossPlatformInputManager.GetButtonDown("Fire1");
        if (!m_ChangeObjectGravity)
            m_ChangeObjectGravity = CrossPlatformInputManager.GetButtonDown("Fire2");
    }


    // Fixed update is called in sync with physics
    private void FixedUpdate()
    {
        // read inputs
        float h = CrossPlatformInputManager.GetAxis("Horizontal");
        float v = CrossPlatformInputManager.GetAxis("Vertical");

        // calculate move direction to pass to character
        // Use movement relative to camera view, or movement relative to player forward direction
        if (m_MovementRelativeToCam)
        {
            m_CamForward = Vector3.Scale(m_Cam.forward, new Vector3(1, 0, 1)).normalized;
            m_Move = v * m_CamForward + h * m_Cam.right;
        }
        else
        {
            //m_Move = v * Vector3.forward + h * Vector3.right;
            m_Move = transform.TransformDirection(new Vector3(h, 0, v).normalized);
        }

        // pass all parameters to the character control script
        m_Character.Move(m_Move, m_Jump, m_ChangeCharacterGravity, m_ChangeObjectGravity);
        m_Jump = false;
        m_ChangeCharacterGravity = false;
        m_ChangeObjectGravity = false;
    }
}
