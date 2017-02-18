using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(CapsuleCollider))]
[RequireComponent(typeof(Player))]
public class PlayerController : MonoBehaviour {

	[SerializeField] float moveSpeed = 6; // move speed
	[SerializeField] float turnSpeed = 90; // turning speed (degrees/second)
	[SerializeField] float lerpSpeed = 10; //smoothing peed
	[SerializeField] float gravity = 10;
	bool isGrounded;
	[SerializeField] float deltaGround = 0.2f; //character is grounded up to this distance
	[SerializeField] float jumpSpeed = 10; //vertical jump initial speed
	[SerializeField] float jumpRange = 10; // the range to detect target wall
	Vector3 surfaceNormal; // current surface normal
	Vector3 myNormal; //character normal
	float distGround; //distance from character position to ground
	bool jumping = false;
	float vertSpeed = 0; //vertical jump current speed

	public BoxCollider boxCollider;
	public GameObject cam;
	public Vector3 camPosini;
	public GameObject playerReference;
    public GameObject gravitySphere;

    //Guillem
    Rigidbody m_Rigidbody;
    CapsuleCollider m_Capsule;
    GravityOnGameObject m_GravityOnPlayer;
    GravityChange m_GravityChange;
    float m_CapsuleHeight;
    Vector3 m_CapsuleCenter;
    private Player m_Player;

	bool a = true;
	bool g = false;

    void Start(){
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Capsule = GetComponent<CapsuleCollider>();
        m_GravityOnPlayer = GetComponent<GravityOnGameObject>();
        m_GravityChange = GetComponent<GravityChange>();
        m_CapsuleHeight = m_Capsule.height;
        m_CapsuleCenter = m_Capsule.center;

        m_Player = GetComponent<Player>();

        myNormal = transform.up; 
		//myTransform = transform;
		GetComponent<Rigidbody> ().freezeRotation = true;

		// distance from transform.position to ground
		distGround = boxCollider.extents.y - boxCollider.center.y;
	}

	void FixedUpdate(){
		//GetComponent<Rigidbody> ().AddForce (-gravity * GetComponent<Rigidbody> ().mass * myNormal);
	}

	void Update(){
		if (Input.GetKey ("1")) {
			a = true;
			g = false;
		}
		if (Input.GetKey ("2")) {
			a = false;
			g = true;
		}

        if (jumping) {
			cam.transform.position = transform.position;
			return;
		}

		//Ray ray;
		//RaycastHit hit;

        //   Debug.DrawRay(transform.position + (transform.up * m_CapsuleHeight / 2), OriginTransform.forward * m_GravityChange.m_MaxDistanceChange, Color.red);
        Vector3 origin_ray = transform.position + (transform.up * m_CapsuleHeight / 2);
        m_GravityChange.DrawRay(origin_ray);

        if (Input.GetButtonDown ("Fire2")) {

            // ray = new Ray (transform.position + transform.up, Camera.main.transform.forward * jumpRange + transform.position);
            jumping = true;
            GetComponent<Rigidbody>().isKinematic = true;
            StartCoroutine(floating(origin_ray));
        }

        if(Input.GetButton("Fire1"))
        {
            gravitySphere.SetActive(true);
            m_GravityChange.ChangeObjectsGravity(origin_ray);
        }
        else
            gravitySphere.SetActive(false);

        // update surface normal and isGrounded
        /* Debug.DrawLine(transform.position, -myNormal + transform.position);
         ray = new Ray(transform.position, -myNormal);
         if (Physics.Raycast (ray, out hit)) {
             isGrounded = hit.distance <= distGround + deltaGround;
             surfaceNormal = hit.normal;
         } else {
             isGrounded = false;
             //surfaceNormal = Vector3.up;
         }*/
        // myNormal = Vector3.Lerp (myNormal, surfaceNormal, lerpSpeed * Time.deltaTime);
		myNormal = Vector3.Lerp (m_GravityOnPlayer.m_Gravity, surfaceNormal, lerpSpeed * Time.deltaTime);
         // find forward direction with new myNormal:
         Vector3 myForward = Vector3.Cross(transform.right, myNormal);
         // align character to the new myNormal while keeping the forward direction:
         Quaternion targetRot = Quaternion.LookRotation(myForward, myNormal);
         transform.rotation = Quaternion.Lerp (transform.rotation, targetRot, lerpSpeed * Time.deltaTime);
         
        //Move the character
        Move();

        //Update cam
        cam.transform.position = transform.position;
        playerReference.transform.position = transform.position;
        playerReference.transform.rotation = cam.transform.localRotation;

    }

    void Move()
    {
        CheckGroundStatus();
        float moveHorizontal = Input.GetAxis("Horizontal");
        float moveVertical = Input.GetAxis("Vertical");

        Vector3 movement;
        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        movement = playerReference.transform.TransformDirection(movement);

        transform.Translate(movement * moveSpeed * Time.deltaTime);

        if (m_Player.m_IsGrounded)
        {
            HandleGroundedMovement(CrossPlatformInputManager.GetButtonDown("Jump"));
        }
        else
        {
            HandleAirborneMovement();
        }
    }

	void JumpToWall(Vector3 point, Vector3 normal){
        jumping = true;
		GetComponent<Rigidbody> ().isKinematic = true;
		Vector3 orgPos = transform.position;
		Quaternion orgRot = transform.rotation;
		Vector3 dstPos = point + normal * (distGround + 0.5f); // will  jump to 0.5 above wall
		Vector3 myForward = Vector3.Cross(transform.right, normal);
		Quaternion dstRot = Quaternion.LookRotation (myForward, normal);

		StartCoroutine (jumpTime (orgPos, orgRot, dstPos, dstRot, normal));
	}

    IEnumerator floating(Vector3 origin_ray)
    {
        bool buttonUp = false;
        for (float t = 0.0f; t < 2.0f;)
        {
            gravitySphere.SetActive(true);
            t += Time.deltaTime;
            if (Input.GetButtonUp("Fire2"))
            {
                buttonUp = true;
                Ray ray;
                RaycastHit hit;
                ray = new Ray(origin_ray, Camera.main.transform.forward * m_GravityChange.m_MaxDistanceChange);
                //jumping = false;
                // GetComponent<Rigidbody>().isKinematic = false;
				if (Physics.Raycast (ray, out hit, jumpRange)) 
				{
					if (g) {
						m_GravityOnPlayer.m_Attractor = hit;
						m_GravityOnPlayer.m_Gravity = (m_Player.transform.position - hit.point).normalized;
						jumping = false;
						GetComponent<Rigidbody> ().isKinematic = false;
						gravitySphere.SetActive (false);
					}
					else
						JumpToWall (hit.point, hit.normal);
				}
                else
                {
                    jumping = false;
                    GetComponent<Rigidbody>().isKinematic = false;
                    gravitySphere.SetActive(false);
                }
                break;
            }

            yield return null;
        }
        if(buttonUp == false)
        {
            jumping = false;
            GetComponent<Rigidbody>().isKinematic = false;
            gravitySphere.SetActive(false);
        }
    }
    IEnumerator jumpTime(Vector3 orgPos, Quaternion orgRot, Vector3 dstPos, Quaternion dstRot, Vector3 normal){
        

        for (float t = 0.0f; t < 1.0f;){
			t += Time.deltaTime;
			transform.position = Vector3.Lerp (orgPos, dstPos, t);
			transform.rotation = Quaternion.Slerp (orgRot, dstRot, t);

			yield return null;
		}
		myNormal = normal;
		GetComponent<Rigidbody> ().isKinematic = false;
		jumping = false;
        //m_GravityOnPlayer.m_Gravity = (m_Player.transform.position - target_wall.point).normalized;
        m_GravityOnPlayer.m_Gravity = myNormal;
        gravitySphere.SetActive(false);
    }


    void HandleGroundedMovement(bool jump)
    {
        // check whether conditions are right to allow a jump:
        //if (jump && m_Animator.GetCurrentAnimatorStateInfo(0).IsName("Grounded"))
        if(jump)
        {
            // jump!
            m_Rigidbody.velocity += m_GravityOnPlayer.m_Gravity * m_Player.m_JumpPower;
            m_Player.m_IsGrounded = false;
            //m_Animator.applyRootMotion = false;
            m_Player.m_GroundCheckDistance = 0.1f;
        }
    }

    void HandleAirborneMovement()
    {
        m_Player.m_GroundCheckDistance = Vector3.Dot(m_Rigidbody.velocity, transform.up) < 0 ? m_Player.m_OrigGroundCheckDistance : 0.01f;
    }

    void CheckGroundStatus()
    {
        RaycastHit hitInfo;
        // helper to visualise the ground check ray in the scene view
        Debug.DrawLine(transform.position + (transform.up * 0.1f), transform.position + (transform.up * 0.1f) + (-transform.up * m_Player.m_GroundCheckDistance), Color.magenta);
        // 0.1f is a small offset to start the ray from inside the character
        // it is also good to note that the transform position in the sample assets is at the base of the character
        if (Physics.Raycast(transform.position + (transform.up * 0.1f), -transform.up, out hitInfo, m_Player.m_GroundCheckDistance))
        {
            m_GravityChange.GravityOnFeet(hitInfo);
            m_Player.m_GroundNormal = transform.InverseTransformVector(hitInfo.normal);
            m_Player.m_IsGrounded = true;
            //m_Animator.applyRootMotion = true;

            // if (!m_DestinationReached)
            m_Player.m_DestinationReached = true;
        }
        else
        {
            m_Player.m_IsGrounded = false;
            m_Player.m_GroundNormal = Vector3.up;
           // m_Animator.applyRootMotion = false;
        }
    }
}