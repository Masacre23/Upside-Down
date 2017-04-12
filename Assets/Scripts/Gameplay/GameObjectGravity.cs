using System.Collections;
using System.Collections.Generic;
using UnityEngine;


//This class should be added to any GameObject which gravity can be changed during the game.
//It controls the current gravity of the object, and adds it to its rigid body.
public class GameObjectGravity : MonoBehaviour {

    Rigidbody m_rigidBody;
    Vector3 m_oldGravity;
    public RaycastHit m_attractor;
    public Vector3 m_gravity;
    public List<Rigidbody> m_planets;
    public bool m_planetGravity;

    bool m_thrownForce = false;
    Vector3 m_impulseForce;

    //This should be the same for all gameobjects
    static float m_gravityStrength = -9.8f;

    void Awake()
    {
        m_planets = new List<Rigidbody>();
    }

	// Use this for initialization
	void Start ()
    {
        m_rigidBody = GetComponent<Rigidbody>();
        m_rigidBody.useGravity = false;
        m_gravity = Physics.gravity;
        m_planetGravity = true;
        m_thrownForce = false;
        m_impulseForce = Vector3.zero;
	}
	
	// Called to add gravity force into the rigid body.
	public void FixedUpdate ()
    {
		float strength = 0;

		if (!m_planetGravity || m_planets.Count == 0)
        {
            strength = m_gravityStrength;
        }
        else
        {
            foreach (Rigidbody planet in m_planets)
            {
                Vector3 newGravity = transform.position - planet.transform.position;
                float distance = newGravity.magnitude;
                newGravity.Normalize();

                float newStrength = - planet.mass / (distance * distance);
                if (newStrength < strength)
                {
                    strength = newStrength;
                    m_gravity = newGravity;
                }     
            }           
        }

        m_rigidBody.AddForce(strength * m_rigidBody.mass * m_gravity);

        if (m_thrownForce)
        {
            m_rigidBody.AddForce(m_impulseForce, ForceMode.VelocityChange);
            m_thrownForce = false;
        }
    }

    //This function is called automatically when this object colliders begins to touch another collider.
    //It's used so when the gameobject reaches it's attractor, it changes its current gravity for the attractor's normal.
    //It's main importance regards objects falling (characters have it's own way to detect floors).
    private void OnCollisionEnter(Collision col)
    {
        if (m_attractor.collider != null)
        {
            if (col.collider == m_attractor.collider)
            {
                m_gravity = m_attractor.normal;
            }
        }
    }

    //This function sets the passed raycasthit as the current attractor for the game object
    //It's mainly to be used by characters in order update its attractor while they walk on an attractor, not on planet
    public void GravityOnFeet(RaycastHit hit)
    {
        if (hit.collider.tag == "GravityWall")
        {
            m_attractor = hit;
            m_gravity = hit.normal;
            m_planetGravity = false;
        }
    }

    //This function sets the GravityObject as a throwing object by the player. 
    public void SetAsThrowingObject(GameObject player)
    {
        if(player != null)
        {
            this.transform.parent = player.transform;
            this.transform.localEulerAngles = new Vector3(0.0f, 0.0f, 0.0f);
            this.transform.localPosition = new Vector3(0.0f, 1.0f, 1.0f);
            this.m_oldGravity = this.m_gravity;
            this.m_gravity = new Vector3(0.0f, 0.0f, 0.0f);
            HingeJoint hingeJoint = this.gameObject.AddComponent<HingeJoint>();
            hingeJoint.connectedBody = player.GetComponent<Rigidbody>();
        }
        else
        {
            HingeJoint hingeJoint = this.gameObject.GetComponent<HingeJoint>();
            Destroy(hingeJoint);
            this.transform.parent = null;
            this.m_gravity = this.m_oldGravity;
        }
       
    }

    //This function is called when we want a object to float. Usually called when player is floating while changing gravity
    //or when an object is floating due the player throwing them.
    public void Float(Vector3 initialPosition, Vector3 finalPosition, float percentage)
    {
        float perc = 2 * percentage - percentage * percentage * percentage;
        m_rigidBody.transform.position = Vector3.Slerp(initialPosition, finalPosition, perc);
    }

    //This function is called when a object is thrown by the player, using PlayerThrowing state.
    public void ThrowObject(Vector3 throwForce)
    {
        m_thrownForce = true;
        m_impulseForce = throwForce;
    }
}
